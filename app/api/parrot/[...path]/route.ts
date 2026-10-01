import { NextRequest, NextResponse } from "next/server";

const PARROT_API_BASE = "https://accounts.parrotproducts.biz/PublicWebServices/Customers.svc";
const cache: Record<string, { data: any; expiry: number }> = {};

function getParrotFeedToken() {
  const token = process.env.PARROT_FEED_TOKEN;
  if (!token) {
    throw new Error(
      "PARROT_FEED_TOKEN is not configured. Set it as a server-only environment variable."
    );
  }
  return token;
}

// Only this read-only catalog feed is reachable through this proxy.
// Every other path/method is rejected before any upstream request is made.
const ALLOWED_GET_ENDPOINTS: Record<string, { buildUpstream: () => string; ttlSeconds: number }> = {
  "/customerProductFeed": {
    buildUpstream: () => `/GetCustomerProductFeed/1/${getParrotFeedToken()}/json`,
    ttlSeconds: 3600,
  },
};

async function fetchParrot(upstreamEndpoint: string) {
  const res = await fetch(`${PARROT_API_BASE}${upstreamEndpoint}`, {
    method: "GET",
    next: { revalidate: 0 },
  });

  if (!res.ok) {
    throw new Error("Supplier upstream request failed");
  }

  return res.json();
}

async function getCached(upstreamEndpoint: string, ttlSeconds: number) {
  const now = Date.now();
  if (cache[upstreamEndpoint] && cache[upstreamEndpoint].expiry > now) {
    return cache[upstreamEndpoint].data;
  }

  const data = await fetchParrot(upstreamEndpoint);
  cache[upstreamEndpoint] = {
    data,
    expiry: now + ttlSeconds * 1000,
  };

  return data;
}

export async function GET(
  _req: NextRequest,
  context: { params: Promise<{ path: string[] }> }
) {
  const { path } = await context.params;
  const endpoint = `/${path.join("/")}`;

  const allowed = ALLOWED_GET_ENDPOINTS[endpoint];
  if (!allowed) {
    return NextResponse.json({ error: "Not found" }, { status: 404 });
  }

  try {
    const upstream = allowed.buildUpstream();
    const data = await getCached(upstream, allowed.ttlSeconds);
    return NextResponse.json(data);
  } catch (error: any) {
    console.error("Parrot API route error:");
    return NextResponse.json({ error: "Supplier request failed" }, { status: 502 });
  }
}

// No POST, PUT or DELETE handlers are exported. Next.js automatically returns
// 405 Method Not Allowed for any method without a handler.

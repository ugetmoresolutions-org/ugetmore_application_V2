import { getAmrodToken } from "@/endpoints/lib/token-manager";
import { NextRequest, NextResponse } from "next/server";

const AMROD_API_BASE = "https://vendorapi.amrod.co.za";
const cache: Record<string, { data: any; expiry: number }> = {};

// Only these read-only catalog endpoints are reachable through this proxy.
// Every other path/method is rejected before the Amrod credentials are ever used.
const ALLOWED_GET_ENDPOINTS: Record<string, { upstream: string; ttlSeconds: number }> = {
  "/productsWithBranding": { upstream: "/api/v1/Products/GetProductsAndBranding", ttlSeconds: 3600 },
  "/prices": { upstream: "/api/v1/Prices", ttlSeconds: 3600 },
  "/brandingPrices": { upstream: "/api/v1/BrandingPrices", ttlSeconds: 3600 },
  "/products": { upstream: "/api/v1/Products", ttlSeconds: 3600 },
  "/stock": { upstream: "/api/v1/Stock", ttlSeconds: 3600 },
  "/categories": { upstream: "/api/v1/Categories", ttlSeconds: 3600 },
};

async function fetchAmrod(upstreamEndpoint: string) {
  const token = await getAmrodToken();

  const res = await fetch(`${AMROD_API_BASE}${upstreamEndpoint}`, {
    method: "GET",
    headers: { Authorization: `Bearer ${token}` },
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

  const data = await fetchAmrod(upstreamEndpoint);
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
    const data = await getCached(allowed.upstream, allowed.ttlSeconds);
    return NextResponse.json(data);
  } catch (error: any) {
    console.error("Amrod API route error:");
    return NextResponse.json({ error: "Supplier request failed" }, { status: 502 });
  }
}

// No POST, PUT or DELETE handlers are exported. Next.js automatically returns
// 405 Method Not Allowed for any method without a handler, so mutating/arbitrary
// requests never reach the Amrod credentials or upstream API.

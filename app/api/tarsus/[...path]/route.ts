import { NextRequest, NextResponse } from "next/server";

const TARSUS_API_BASE = "https://feedgen.tarsusonline.co.za/api/DataFeed";
const TARSUS_API_KEY = process.env.TARSUS_API_KEY; // Store your key in .env.local

// Only this read-only catalog path is reachable through this proxy.
const ALLOWED_GET_PATHS = new Set(["/Customer-ProductCatalogue"]);

async function makeTarsusRequest(endpoint: string) {
  if (!TARSUS_API_KEY) {
    throw new Error("TARSUS_API_KEY is not configured");
  }

  const response = await fetch(`${TARSUS_API_BASE}${endpoint}`, {
    method: 'GET',
    headers: {
      'Authorization': `Bearer ${TARSUS_API_KEY}`,
      'Content-Type': 'application/json',
    },
    next: { revalidate: 3600 }, // Cache for 1 hour
  });

  if (!response.ok) {
    throw new Error("Supplier upstream request failed");
  }

  return response.json();
}

export async function GET(
  _req: NextRequest,
  context: { params: Promise<{ path: string[] }> }
) {
  const { path } = await context.params;
  const endpoint = `/${path.join("/")}`;

  if (!ALLOWED_GET_PATHS.has(endpoint)) {
    return NextResponse.json({ error: "Not found" }, { status: 404 });
  }

  try {
    const data = await makeTarsusRequest(endpoint);
    return NextResponse.json(data);
  } catch (error: any) {
    console.error("Tarsus API route error:");
    return NextResponse.json({ error: "Supplier request failed" }, { status: 502 });
  }
}

// No POST, PUT or DELETE handlers are exported. Next.js automatically returns
// 405 Method Not Allowed for any method without a handler.

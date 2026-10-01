import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";

const ORIGINAL_ENV = { ...process.env };

describe("Parrot supplier proxy route", () => {
  let fetchMock: ReturnType<typeof vi.fn>;

  beforeEach(() => {
    vi.resetModules();
    process.env = { ...ORIGINAL_ENV, PARROT_FEED_TOKEN: "test-feed-token" };
    fetchMock = vi.fn().mockResolvedValue({
      ok: true,
      json: async () => ({ Products: [] }),
      text: async () => "",
    });
    vi.stubGlobal("fetch", fetchMock);
  });

  afterEach(() => {
    process.env = { ...ORIGINAL_ENV };
    vi.unstubAllGlobals();
    vi.clearAllMocks();
  });

  it("exports no POST, PUT or DELETE handlers, so Next.js rejects those methods with 405 before any supplier call is possible", async () => {
    const route = await import("@/app/api/parrot/[...path]/route");

    expect((route as any).POST).toBeUndefined();
    expect((route as any).PUT).toBeUndefined();
    expect((route as any).DELETE).toBeUndefined();
  });

  it("rejects an anonymous GET to a path outside the allowlist without contacting Parrot", async () => {
    const { GET } = await import("@/app/api/parrot/[...path]/route");

    const response = await GET(
      {} as any,
      { params: Promise.resolve({ path: ["anything"] }) }
    );

    expect(response.status).toBe(404);
    expect(fetchMock).not.toHaveBeenCalled();
  });

  it("serves the allowlisted customer feed using the feed token from environment variables, not a hardcoded value", async () => {
    const { GET } = await import("@/app/api/parrot/[...path]/route");

    const response = await GET(
      {} as any,
      { params: Promise.resolve({ path: ["customerProductFeed"] }) }
    );

    expect(response.status).toBe(200);
    expect(fetchMock).toHaveBeenCalledTimes(1);

    const [url] = fetchMock.mock.calls[0];
    expect(url).toBe(
      "https://accounts.parrotproducts.biz/PublicWebServices/Customers.svc/GetCustomerProductFeed/1/test-feed-token/json"
    );
  });

  it("fails clearly when PARROT_FEED_TOKEN is not configured, instead of silently falling back to a hardcoded value", async () => {
    process.env.PARROT_FEED_TOKEN = "";

    const { GET } = await import("@/app/api/parrot/[...path]/route");

    const response = await GET(
      {} as any,
      { params: Promise.resolve({ path: ["customerProductFeed"] }) }
    );

    expect(response.status).toBe(502);
    expect(fetchMock).not.toHaveBeenCalled();
  });
});

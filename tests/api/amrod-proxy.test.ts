import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";

vi.mock("@/endpoints/lib/token-manager", () => ({
  getAmrodToken: vi.fn().mockResolvedValue("mock-amrod-token"),
}));

describe("Amrod supplier proxy route", () => {
  let fetchMock: ReturnType<typeof vi.fn>;

  beforeEach(() => {
    vi.resetModules();
    fetchMock = vi.fn().mockResolvedValue({
      ok: true,
      json: async () => ({ products: [] }),
      text: async () => "",
    });
    vi.stubGlobal("fetch", fetchMock);
  });

  afterEach(() => {
    vi.unstubAllGlobals();
    vi.clearAllMocks();
  });

  it("exports no POST, PUT or DELETE handlers, so Next.js rejects those methods with 405 before any supplier call is possible", async () => {
    const route = await import("@/app/api/amrod/[...path]/route");

    expect((route as any).POST).toBeUndefined();
    expect((route as any).PUT).toBeUndefined();
    expect((route as any).DELETE).toBeUndefined();
  });

  it("rejects an anonymous GET to a path outside the allowlist without contacting Amrod", async () => {
    const { GET } = await import("@/app/api/amrod/[...path]/route");

    const response = await GET(
      {} as any,
      { params: Promise.resolve({ path: ["admin", "delete-everything"] }) }
    );

    expect(response.status).toBe(404);
    expect(fetchMock).not.toHaveBeenCalled();
  });

  it("serves an allowlisted catalog endpoint and attaches the bearer token only to the known upstream path", async () => {
    const { GET } = await import("@/app/api/amrod/[...path]/route");

    const response = await GET(
      {} as any,
      { params: Promise.resolve({ path: ["products"] }) }
    );

    expect(response.status).toBe(200);
    expect(fetchMock).toHaveBeenCalledTimes(1);

    const [url, options] = fetchMock.mock.calls[0];
    expect(url).toBe("https://vendorapi.amrod.co.za/api/v1/Products");
    expect(options.headers.Authorization).toBe("Bearer mock-amrod-token");
  });

  it("still serves the other five allowlisted catalog endpoints correctly", async () => {
    const { GET } = await import("@/app/api/amrod/[...path]/route");

    const cases: Array<[string, string]> = [
      ["productsWithBranding", "https://vendorapi.amrod.co.za/api/v1/Products/GetProductsAndBranding"],
      ["prices", "https://vendorapi.amrod.co.za/api/v1/Prices"],
      ["brandingPrices", "https://vendorapi.amrod.co.za/api/v1/BrandingPrices"],
      ["stock", "https://vendorapi.amrod.co.za/api/v1/Stock"],
      ["categories", "https://vendorapi.amrod.co.za/api/v1/Categories"],
    ];

    for (const [incoming, expectedUpstream] of cases) {
      fetchMock.mockClear();
      const response = await GET(
        {} as any,
        { params: Promise.resolve({ path: [incoming] }) }
      );
      expect(response.status).toBe(200);
      expect(fetchMock).toHaveBeenCalledWith(expectedUpstream, expect.any(Object));
    }
  });
});

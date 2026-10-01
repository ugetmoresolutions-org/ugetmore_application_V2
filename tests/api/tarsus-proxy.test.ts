import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";
import { randomUUID } from 'node:crypto';

const ORIGINAL_ENV = { ...process.env };

describe("Tarsus supplier proxy route", () => {
  let fetchMock: ReturnType<typeof vi.fn>;

  beforeEach(() => {
    vi.resetModules();
    process.env = { ...ORIGINAL_ENV, TARSUS_API_KEY: randomUUID() };
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
    const route = await import("@/app/api/tarsus/[...path]/route");

    expect((route as any).POST).toBeUndefined();
    expect((route as any).PUT).toBeUndefined();
    expect((route as any).DELETE).toBeUndefined();
  });

  it("rejects an anonymous GET to a path outside the allowlist without contacting Tarsus", async () => {
    const { GET } = await import("@/app/api/tarsus/[...path]/route");

    const response = await GET(
      {} as any,
      { params: Promise.resolve({ path: ["SomeOtherFeed"] }) }
    );

    expect(response.status).toBe(404);
    expect(fetchMock).not.toHaveBeenCalled();
  });

  it("serves the allowlisted catalogue path with the API key attached", async () => {
    const { GET } = await import("@/app/api/tarsus/[...path]/route");

    const response = await GET(
      {} as any,
      { params: Promise.resolve({ path: ["Customer-ProductCatalogue"] }) }
    );

    expect(response.status).toBe(200);
    expect(fetchMock).toHaveBeenCalledTimes(1);

    const [url, options] = fetchMock.mock.calls[0];
    expect(url).toBe("https://feedgen.tarsusonline.co.za/api/DataFeed/Customer-ProductCatalogue");
    expect(options.headers.Authorization).toBe(`Bearer ${process.env.TARSUS_API_KEY}`);
  });
});

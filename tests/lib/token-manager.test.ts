import { describe, it, expect, vi, beforeEach, afterEach } from "vitest";

const ORIGINAL_ENV = { ...process.env };

describe("Amrod token manager", () => {
  beforeEach(() => {
    vi.resetModules();
    process.env = { ...ORIGINAL_ENV };
  });

  afterEach(() => {
    process.env = { ...ORIGINAL_ENV };
    vi.restoreAllMocks();
  });

  it("refuses to fetch a token when Amrod credentials are not configured", async () => {
    delete process.env.AMROD_USERNAME;
    delete process.env.AMROD_PASSWORD;
    delete process.env.AMROD_CUSTOMER_CODE;

    const { getAmrodToken } = await import("@/endpoints/lib/token-manager");

    await expect(getAmrodToken()).rejects.toThrow(/not configured/i);
  });

  it("fetches a token using credentials read from environment variables, not hardcoded source values", async () => {
    process.env.AMROD_USERNAME = "test-user";
    process.env.AMROD_PASSWORD = "test-pass";
    process.env.AMROD_CUSTOMER_CODE = "000000";

    vi.doMock("axios", () => ({
      default: {
        post: vi.fn().mockResolvedValue({
          data: { token: "mock-token", expiry: 3600 },
        }),
      },
    }));

    const axios = (await import("axios")).default as any;
    const { getAmrodToken } = await import("@/endpoints/lib/token-manager");

    const token = await getAmrodToken();

    expect(token).toBe("mock-token");
    expect(axios.post).toHaveBeenCalledWith(
      "https://identity.amrod.co.za/VendorLogin",
      { username: "test-user", password: "test-pass", customerCode: "000000" },
      expect.any(Object)
    );
  });
});

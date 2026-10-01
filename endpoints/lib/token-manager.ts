// lib/token-manager.ts
import axios from 'axios';

// Token management
export interface TokenCache {
  token: string;
  expiry: number;
  fetchedAt: number;
}

// Global token cache - in production, consider using Redis or similar for multiple instances
let tokenCache: TokenCache | null = null;

const LOGIN_ENDPOINT = "https://identity.amrod.co.za/VendorLogin";

function getAmrodCredentials() {
  const username = process.env.AMROD_USERNAME;
  const password = process.env.AMROD_PASSWORD;
  const customerCode = process.env.AMROD_CUSTOMER_CODE;

  if (!username || !password || !customerCode) {
    throw new Error(
      "Amrod credentials are not configured. Set AMROD_USERNAME, AMROD_PASSWORD and AMROD_CUSTOMER_CODE as server-only environment variables."
    );
  }

  return { username, password, customerCode };
}

export async function getAmrodToken(): Promise<string> {
  const now = Date.now();

  // Check if we have a valid cached token (with 10 minute buffer before expiry)
  if (tokenCache && (now - tokenCache.fetchedAt) < (tokenCache.expiry - 600) * 1000) {
    return tokenCache.token;
  }

  const credentials = getAmrodCredentials();

  try {
    const response = await axios.post(LOGIN_ENDPOINT, credentials, {
      headers: { "Content-Type": "application/json" }
    });

    const { token, expiry } = response.data;

    tokenCache = {
      token,
      expiry,
      fetchedAt: now
    };

    return token;
  } catch (error: any) {
    console.error("Failed to fetch Amrod token:", error.message);
    throw new Error("Authentication failed");
  }
}

export function getTokenCache(): TokenCache | null {
  return tokenCache;
}

export function clearTokenCache(): void {
  tokenCache = null;
  console.log("Token cache cleared");
}


export function getTokenStatus(): { hasToken: boolean; isExpired: boolean; expiresIn?: number } {
  if (!tokenCache) {
    return { hasToken: false, isExpired: true };
  }
  
  const now = Date.now();
  const ageInSeconds = (now - tokenCache.fetchedAt) / 1000;
  const isExpired = ageInSeconds >= tokenCache.expiry;
  const expiresIn = tokenCache.expiry - ageInSeconds;
  
  return {
    hasToken: true,
    isExpired,
    expiresIn: isExpired ? 0 : Math.max(0, expiresIn)
  };
}
const test = require('node:test');
const assert = require('node:assert/strict');
const { loadSource } = require('./helpers.cjs');

function middlewareForTest() {
  return loadSource('middleware.ts', {
    'next/server': { NextResponse: {
      next: () => ({ allowed: true }),
      redirect: () => ({ allowed: false, cookies: { delete() {} } }),
    } },
    'jwt-decode': require('jwt-decode'),
  }).middleware;
}

test('anonymous users cannot enter admin routes', () => {
  const response = middlewareForTest()({
    cookies: { get: () => undefined }, nextUrl: new URL('https://example.test/admin/dashboard'),
  });
  assert.equal(response.allowed, false);
});

// These are required security expectations, intentionally red while F03/F04 remain open.
// Do not skip them or change expected values to approve the vulnerable baseline.
test('F03: unsigned admin claims must not authorize an admin route', () => {
  const header = Buffer.from(JSON.stringify({ alg: 'none' })).toString('base64url');
  const payload = Buffer.from(JSON.stringify({ role: 'admin', exp: 4102444800 })).toString('base64url');
  const response = middlewareForTest()({
    cookies: { get: () => ({ value: `xxxx${header}.${payload}.` }) },
    nextUrl: new URL('https://example.test/admin/dashboard'),
  });
  assert.equal(response.allowed, false, 'Unverified claims cannot establish identity');
});

test('F04: supplier HTML must not retain executable event attributes', () => {
  const { sanitizeHtmlContent } = loadSource('utils/productStorage.ts');
  const result = sanitizeHtmlContent('<img src="x" onerror="alert(1)">');
  assert.doesNotMatch(result, /\bonerror\s*=/i, 'Sanitize untrusted descriptions before HTML rendering');
});

# PR 1 remediation on 1 October 2026

PR #1 incorporates the existing commits from [PR #2](https://github.com/ugetmoresolutions-org/ugetmore_application_V2/pull/2) without merging either PR into main. Its supplier allowlists and environment configuration are preserved. The original checkout is not modified by this work.

## Changes

- Run both the Node regression suite and the Vitest supplier suite from `npm test`. A failure in either returns a failing exit code, but does not prevent the other suite from running.
- Sanitize supplier descriptions with a parser and an explicit basic-formatting allowlist. Drop active elements and attributes; do not decode escaped tags into executable HTML.
- Remove browser token logging and supplier error details from proxy logs. Regression tests cover upstream errors containing private feed information.
- Generate synthetic test credentials at runtime, rather than storing credential-shaped test literals. This does not exempt test files from secret scanning.
- Keep current-source and full-history secret scans as separate required steps. A clean current checkout is not evidence that an old credential was revoked.

## Dependency decisions

- Use patched Next.js 15 and matching ESLint configuration, retaining the existing application architecture.
- Restore Flowbite React 0.12 with Tailwind 4 support. PR #2's 0.10 downgrade was incompatible with the existing plugin configuration. Commit the CSS import required by the current plugin.
- Override Flowbite's transitive `deepmerge-ts` to 8.0.2. The [publisher's release notes](https://github.com/RebeccaStevens/deepmerge-ts/releases/tag/v8.0.0) describe the recursion fix and changed Map semantics. Application themes use plain objects; a rendered Flowbite custom-theme regression checks this integration.
- Override Next's transitive PostCSS to 8.5.28 to address the audit findings without an unrelated Next 16 migration. Validate through the production build.
- Pin react-multi-carousel to 2.8.5, avoiding the npm CLI dependency introduced by 2.8.6. Remove unused ExcelJS; repository imports use SheetJS instead.
- Install SheetJS 0.20.3 from its [official distribution](https://docs.sheetjs.com/docs/getting-started/installation/nodejs/), with integrity recorded in the lockfile. Tests round-trip XLSX, XLS and ODS files through the importer APIs. Do not replace this with the outdated npm-registry release.
- Use Node 22 types to match CI and Vitest's supported peer dependency. No force install or legacy-peer-deps exception is used.

## Remaining external prerequisites

**JWT verification:** PR #2 explicitly leaves SEC-002 unresolved. Obtain the backend authentication contract: issuer, audience, signing algorithm and public verification key/JWKS URL, or an authenticated session-verification endpoint. Do not invent an endpoint or accept decoded JWT claims as identity. The F03 test remains blocking until signature/session verification is implemented and valid, expired, forged and wrong-role tokens are tested against that contract.

**Supplier revocation and history:** PR #2 removes current source credentials but explicitly excludes rotation. The supplier account owner must confirm Amrod credential and token revocation, and Parrot feed-token rotation as applicable. Full-history scanning still reports the original Amrod credential; PR #2 also introduced three historical synthetic-fixture matches. They are not three additional exposed production credentials. Current fixture generation eliminates new matches, but does not rewrite those old commits.

Historical cleanup or a narrowly documented policy for confirmed revoked credentials requires independent review and coordination with collaborators. This PR does not rewrite shared history, allowlist the real credential, or disable full-history scanning. Rotation alone will not remove the historical scanner match. Do not claim that simply setting environment variables makes CI green.

**Independent review:** AI review remains blocked behind secret scanning, and human approval remains required. Neither code changes nor passing build checks establish production readiness.

See [PROJECT_STATE.md](../PROJECT_STATE.md) for the latest validation results and next action.

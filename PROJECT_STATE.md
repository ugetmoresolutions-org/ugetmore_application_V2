---
project: UGETMORE e-commerce platform
state_version: 1
last_updated: "2026-09-17T11:17:21+02:00"
updated_by: Codex, at repository owner's request
current_phase: Assessment complete; security and commerce stabilization planned
system_maturity: mvp
target_maturity: production
active_work_unit: null
repository_branch: main
repository_revision: d91c2b58f2634e7caf88e5060684fd4c9c692c68
revision_scope: Assessed application baseline; this documentation commit is recorded by Git
overall_status: IN_PROGRESS
production_readiness: BLOCKING
---

# Project state

> Read this file before substantial work. Verify relevant code, dependencies and ownership before trusting its status. Claim a bounded work unit before editing shared files. After meaningful progress, record validation evidence, decisions and blockers; increment the state version and timestamp; end with one precise next action. Never equate implemented, validated, deployed and production-verified.

## 01. State metadata

This is the authoritative running project snapshot under AI-SWE-002A. It records known reality, not release approval. `mvp` describes the implementation's orientation: a complete, validated MVP baseline has **not** been established. Production is the assessment target, pending confirmation of business requirements.

Detailed evidence and findings F01–F14: [repository assessment, 17 September 2026](docs/repository-assessment-2026-09-17.md). That report is a dated assessment, not a competing running state file. Git history identifies the commit containing this snapshot.

## 02. Project objective

Provide an e-commerce storefront for individual and business buyers, including supplier products, branded merchandise and school stationery packs. Support browsing, carts, coupons, checkout, customer orders, artwork review and administrative product/order management.

Delivery objective: stabilize the existing application and demonstrate secure, reliable commerce before production approval. Exact release scope, traffic targets, budget and operational ownership remain unconfirmed.

## 03. Current system snapshot

| Component | Current evidence |
| --- | --- |
| Frontend | Next.js/React storefront and admin UI; 42 page files at assessed baseline. Authentication screens, catalog, cart, school packs, branding and order interfaces exist. End-to-end feature acceptance is unverified. |
| Backend | External business API called by resource wrappers. Backend source and API specification have not been assessed. |
| Data | TypeScript interfaces, browser localStorage/IndexedDB and external persistence assumptions. Actual schemas, constraints and migrations unavailable. |
| Infrastructure | Local production build previously passed. Five Next.js API route files implement supplier proxies and Amrod cache/status operations. Live infrastructure unverified. |
| Integrations | Amrod, Parrot, Tarsus and backend-mediated PayFast payment generation. Live permissions, callbacks and reconciliation unverified. |

No remediation from the assessment has been validated. Local dependency edits were observed during state preparation; see sections 09 and 15.

## 04. Architecture snapshot

- **Frontend:** Next.js App Router, React, strict TypeScript, Tailwind, Flowbite/Radix components, Axios and browser storage.
- **Business boundary:** browser and Next.js product loaders call the external API configured by `NEXT_PUBLIC_URL`; absent configuration falls back to localhost:5000. Authentication and authoritative commerce rules must be enforced by trusted server code.
- **Supplier boundary:** Next.js proxies call supplier APIs. Amrod/Parrot caches and Amrod token cache are instance-local; Tarsus uses fetch revalidation.
- **Payment boundary:** frontend requests a payment URL from the business backend, then redirects. Payment confirmation implementation is outside this repo.
- **Database, upload storage, messaging and hosting:** actual backend/runtime arrangements are not verified. Do not infer database security or hosting readiness from frontend interfaces or Vercel references.

## 05. Current phase

**Phase:** security and commerce stabilization planning. **Status:** PLANNED.

The repository assessment is complete. Phase exit requires validated supplier access controls and session verification; safe HTML handling; recoverable checkout; authoritative price/stock/payment contracts; dependency validation; and regression evidence for critical failure paths. Operational release requirements remain additional gates.

## 06. Active work unit

**Primary active unit:** none after this documentation handoff. Remediation owners are unassigned; no remediation is claimed as in progress by this file.

**STATE-001 — Establish collaborator state:** documentation scope is `PROJECT_STATE.md` and the linked assessment. Acceptance: all 19 state sections present, findings accurately qualified, no secrets/customer records included, links resolve, and only intended documentation is committed. Publication is verified through Git remote history; this snapshot does not claim an application deployment.

Before taking the next unit, assign an owner and record its scope. The observed package edits have unknown ownership; coordinate before modifying them.

## 07. Validated completed work

| Unit | Status | Evidence and limit |
| --- | --- | --- |
| REPO-001 — Repository access | VALIDATED | Authenticated GitHub repository access and remote HEAD checked; assessed application revision recorded above. |
| ASSESS-001 — Repository assessment | VALIDATED | Architecture inventory, targeted commerce/security review, all 13 AI-SWE capabilities and full readiness gate documented in linked report. Assessment completion does not mean defects are resolved. |
| CHECK-001 — Development checks on assessed working tree | VALIDATED | `npm run build`, `npm run lint` and TypeScript no-emit check passed on 17 September before subsequent dependency edits. Not a clean-checkout or current dependency validation. |

No payment, database isolation, deployment, restoration or production workflow is marked validated.

## 08. Pending work

| Unit | Status | Scope / completion evidence |
| --- | --- | --- |
| SEC-001 | READY | Restrict supplier proxy methods/paths, protect privileged operations, externalize supplier credentials. Offline tests must show anonymous privileged calls cannot reach supplier handlers. Credential rotation tracked separately below. |
| SEC-002 | PLANNED | Verify sessions at trusted boundaries, sanitize descriptions, remove sensitive token handling/logging. Invalid tokens rejected; hostile descriptions inert. Requires backend auth contract for integration. |
| COM-001 | READY | Fix failed-checkout modal, response/error contract and retry/cancel behavior. Mock payment generation failure and confirm UI recovery. |
| COM-002 | PLANNED | Remove fabricated price/stock; unify cart/document totals; persist guest artwork durably; fix search fallback/cancellation. Cover boundary and reload cases. |
| DEP-001 | READY | Identify owner/intent of local package changes; review compatibility and reconcile lockfile before clean-install, build, lint, typecheck and audit. |
| API-001 | BLOCKED | Inspect backend identity, ownership, stock/pricing, order transactions and payment notification/idempotency controls. Needs backend source/specification and safe test environment. |
| QA-001 | PLANNED | Add regression tests and CI for security, commerce and integration failure paths; validate mobile and accessibility flows. |
| OPS-001 | BLOCKED | Verify staging/production, monitoring, backups, restore and rollback. Needs runtime access, owners and operational evidence. |

Infrastructure expansion and broad refactoring are DEFERRED until requirements or measured bottlenecks justify them. READY means actionable scope, not that an owner has started it or that provider/account changes have been approved.

## 09. Dependency state

- SEC-001 code containment and COM-001 UI recovery can proceed independently of backend discovery. Use isolated fixtures instead of production supplier/payment mutations.
- Integrated SEC-002 and COM-002 approval require the relevant API-001 contracts. Release approval requires all applicable security, commerce, QA and operations gates.
- Baseline manifest: Next 15.4.10, React 19.1.0. At snapshot preparation, **uncommitted** manifest changes request Next `^15.5.25`, Flowbite React `^0.10.2`, ExcelJS `^3.4.0` and react-multi-carousel `^2.8.5`, with a modified lockfile. These edits are **IMPLEMENTED_UNVERIFIED**, owner unknown, and excluded from this documentation publication. Do not treat requested versions as installed or tested versions.
- The earlier audit found 26 affected package entries (1 critical, 19 high, 5 moderate, 1 low). This is historical evidence for the assessed dependency state; a new audit is required after reconciliation.

## 10. Decision register

| ID | Status | Decision / reason / impact |
| --- | --- | --- |
| DEC-001 | ACTIVE | Use root `PROJECT_STATE.md` as the single running snapshot, with dated reports for detailed evidence. Requested for collaborator handoff under AI-SWE-002A. |
| DEC-002 | ACTIVE | Publish this documentation independently of unrelated package edits, preserving their ownership and validation boundary. |
| DEC-003 | PROPOSED | Stabilize the existing Next.js/external-API architecture before broad redesign. Current evidence does not justify distributed infrastructure expansion. |
| DEC-004 | PROPOSED | Make the backend authoritative for identity, resource ownership, prices, stock, coupons and paid orders. Validate against actual backend implementation before choosing integration changes. |

Record superseding decisions explicitly; do not overwrite prior architectural decisions silently.

## 11. Constraints

- Current evidence covers this frontend/proxy repository; backend and deployed controls cannot be certified from it.
- Preserve unrelated or concurrent edits; stage explicit files. Do not include credentials, customer records or raw sensitive logs in state updates.
- Assessment findings do not authorize live supplier mutations or payment transactions. Use safe fixtures/test environments for validation.
- Clean lint is limited by disabled hook rules; compilation is not functional or security acceptance.

## 12. Assumptions

| ID | Status | Assumption / impact |
| --- | --- | --- |
| ASM-001 | UNVERIFIED | Level 2 production readiness is the intended target for real commerce. Confirm release scope and service expectations. |
| ASM-002 | UNVERIFIED | Backend is intended to own persistent carts, orders, payments and user isolation. Obtain implementation evidence; frontend calls do not prove enforcement. |
| ASM-003 | UNVERIFIED | Existing architecture may meet initial demand. No traffic, catalog-size or latency target has been agreed or tested. |

## 13. Known issues

All findings below remain **OPEN** unless a later state version records fix evidence. F-identifiers map to exact references in the assessment.

| Findings | Severity | Impact / affected approval |
| --- | --- | --- |
| F01–F02 | CRITICAL | Committed supplier credentials and unauthenticated credential-bearing proxy; block safe public exposure. Credential validity was not tested. |
| F03–F05 | HIGH | Unverified JWT authorization, unsafe description HTML and browser-readable token handling; block security approval. Backend compromise has not been demonstrated. |
| F06–F07 | HIGH | Checkout failure trap and invented fallback price/stock; block commerce approval. |
| F08 | CRITICAL | Historical dependency audit includes a critical package rating; exploit conditions vary. Pending dependency changes have not been validated. |
| F09–F11 | MEDIUM | Search fallback/cancellation, guest artwork persistence and misleading Amrod cache/status operations. |
| F12–F14 | MEDIUM | Shipping/document discrepancies, synthetic pre-payment receipts and inconsistent error contracts. |

## 14. Technical debt

- **DEBT-001 — DEFERRED:** large UI modules (branding details ~3,509 lines; coupons ~3,028), overlapping types and orchestration in components/hooks. Split around tested domain contracts after urgent correctness work.
- **DEBT-002 — PLANNED:** restore meaningful hook lint rules, simplify overlapping dependencies and document configuration. Target DEP-001/QA-001; avoid blanket suppression as a validation substitute.
- **DEBT-003 — DEFERRED:** define cache consistency and multi-instance behavior when workload/runtime requirements are known. Do not introduce shared infrastructure without a demonstrated need.

## 15. Validation state

| Area | Evidence / current limit |
| --- | --- |
| Build, lint, TypeScript | Historical PASS for assessed working tree; current dependency edits NOT RUN. Commands in section 07 and assessment. |
| Unit/integration/E2E | No tracked automated functional suite found during assessment; live workflows NOT RUN. |
| Security | FAIL at assessed baseline: isolated probes allowed a fabricated admin cookie and preserved an HTML event handler. These probes used local source/mocks, not live exploitation. No corrective evidence yet. |
| Dependencies | Historical audit described in section 09. New package state UNVERIFIED. |
| Database/payment | NOT VERIFIED: permissions, transactions, webhook validation, idempotency and reconciliation require backend review. |
| Performance | Bundle metrics only; product detail first-load JS ~280 kB. Browser/load capacity NOT TESTED. |
| Recovery/deployment | NOT VERIFIED. No restore, rollback or deployment acceptance performed. |
| Production readiness | BLOCKING. The full gate is in the assessment; no production-ready claim is supported. |

Historical checks used an already modified lockfile/installed dependencies. Build tooling selected an ancestor lockfile and automatically rewrote a Flowbite CSS directive, later restored. The environment emitted a warning that Node TLS verification was disabled; that setting was not introduced by the assessment. Reproduce checks in a clean, correctly configured environment before release.

## 16. Environment & deployment state

- **Development:** local build previously completed; current dependency state needs revalidation.
- **Test/staging/production:** availability, access and configuration UNKNOWN; do not label absent or healthy without inspection.
- **CI/CD:** no tracked workflow found at assessment baseline; external automation/repository settings not inspected.
- **Latest application deployment:** UNKNOWN. Pushing these documents is not an application deployment or proof that external auto-deployment succeeded.
- **Database, HTTPS, secrets, backups, restoration and rollback:** operational evidence outstanding.

## 17. Recent material changes

**2026-09-17:** repository access verified; comprehensive assessment and local baseline checks completed; security/commerce findings recorded. Subsequent local manifest/lockfile changes observed with unknown ownership and no fresh validation. Added this state snapshot and linked assessment for GitHub collaboration; no application remediation is included in this documentation change.

## 18. Blockers

| ID | Affected work | Required resolution |
| --- | --- | --- |
| BLOCK-001 | Security/release approval | Restrict exposed proxies and rotate/revoke committed supplier credentials through the account owner; validate replacement configuration. Repository code fixes alone do not revoke historical credentials. |
| BLOCK-002 | API-001; integrated auth/payment approval | Provide backend repository/specification and staging test setup; demonstrate ownership, price/stock enforcement and payment notification/idempotency behavior. |
| BLOCK-003 | OPS-001; production approval | Identify infrastructure owner and produce hosting, HTTPS, monitoring, backup/restore and rollback evidence. |
| BLOCK-004 | Reusing historical validation for package edits | Establish package-change ownership, reconcile intended versions and run clean validation. This does not prevent independent documentation or isolated source fixes. |

## 19. Next action

**Recommended next work unit: SEC-001. Owner: unassigned.** Claim the unit, inspect supplier call sites, enumerate only the routes/methods actually needed, and replace unrestricted proxy forwarding with explicit permitted operations and verified access for privileged actions. Externalize credentials and coordinate rotation with the supplier account owner without copying values into documentation.

**Validation:** offline tests reject anonymous privileged operations, unknown paths/methods and invalid input without contacting suppliers; permitted catalog requests still map correctly. Run build/typecheck against an agreed dependency baseline.

**Transition condition:** mark code containment VALIDATED only after checks pass; keep credential rotation and release approval blocked until their separate evidence is recorded. Update this file with owner, files, results and the next bounded action.

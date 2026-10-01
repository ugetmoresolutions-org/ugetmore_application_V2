---
project: UGETMORE e-commerce platform
state_version: 6
last_updated: "2026-10-01"
updated_by: Codex, at repository owner's request
current_phase: Mandatory merge rules active; baseline remediation blocked
system_maturity: mvp
target_maturity: production
active_work_unit: CI-001
repository_branch: codex/github-actions-gates
repository_revision: cc01c4f1d1d16e4bc49361bfb900da98c7b1027b
revision_scope: Application and workflow code validated by hosted run 36856476251; subsequent changes are documentation only
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

PR #2 supplier containment is integrated into the CI branch. HTML sanitization and safe supplier-error logging are implemented with local regression evidence. JWT verification, historical credential revocation/cleanup and release acceptance remain blocked. The original checkout is preserved. See sections 09 and 15.

## 04. Architecture snapshot

- **Frontend:** Next.js App Router, React, strict TypeScript, Tailwind, Flowbite/Radix components, Axios and browser storage.
- **Business boundary:** browser and Next.js product loaders call the external API configured by `NEXT_PUBLIC_URL`; absent configuration falls back to localhost:5000. Authentication and authoritative commerce rules must be enforced by trusted server code.
- **Supplier boundary:** Next.js proxies call supplier APIs. Amrod/Parrot caches and Amrod token cache are instance-local; Tarsus uses fetch revalidation.
- **Payment boundary:** frontend requests a payment URL from the business backend, then redirects. Payment confirmation implementation is outside this repo.
- **Database, upload storage, messaging and hosting:** actual backend/runtime arrangements are not verified. Do not infer database security or hosting readiness from frontend interfaces or Vercel references.

## 05. Current phase

**Phase:** automated engineering gates. **Status:** BLOCKED on existing security defects and independent approval. The owner requested GitHub Actions and mandatory pre-merge evidence under AI-SWE-006.

The repository assessment is complete. Phase exit requires validated supplier access controls and session verification; safe HTML handling; recoverable checkout; authoritative price/stock/payment contracts; dependency validation; and regression evidence for critical failure paths. Operational release requirements remain additional gates.

## 06. Active work unit

**Primary active unit: CI-001 — Automated engineering gates. Implementation: Codex; active owner: Codex. Status: BLOCKED. Integrating PR #2 supplier containment into PR #1, reconciling both test suites, fixing HTML sanitization and dependency failures. JWT contract and supplier revocation remain external prerequisites.** Workflow publication/execution verified in [PR #1](https://github.com/ugetmoresolutions-org/ugetmore_application_V2/pull/1); mandatory branch rules are active and effective on main. The PR remains unmerged.

Scope: workflow, starter commerce/security tests, review policy, PR template, CODEOWNERS, owner-installable main ruleset, dependency update configuration and operating documentation. Supplier containment, HTML sanitization and dependency fixes are now included; no automatic merge/deploy. Acceptance: workflow syntax valid; clean-install checks executed; negative security tests expose known defects; aggregate gate fails on failing prerequisites; PR run observed; owner enables rules before technical merge enforcement is claimed. GitHub now reports admin access for Mnqobi-Developer. Ruleset 23605355 is active; effective main rules were verified and PR #1 reports BLOCKED / REVIEW_REQUIRED.

**STATE-001 — Establish collaborator state:** documentation scope is `PROJECT_STATE.md` and the linked assessment. Acceptance: all 19 state sections present, findings accurately qualified, no secrets/customer records included, links resolve, and only intended documentation is committed. Publication is verified through Git remote history; this snapshot does not claim an application deployment.

Before taking the next unit, assign an owner and record its scope. The observed package edits have unknown ownership; coordinate before modifying them.

## 07. Validated completed work

| Unit | Status | Evidence and limit |
| --- | --- | --- |
| REPO-001 — Repository access | VALIDATED | Authenticated GitHub repository access and remote HEAD checked; assessed application revision recorded above. |
| ASSESS-001 — Repository assessment | VALIDATED | Architecture inventory, targeted commerce/security review, all 13 AI-SWE capabilities and full readiness gate documented in linked report. Assessment completion does not mean defects are resolved. |
| CHECK-001 — Development checks on assessed working tree | VALIDATED | `npm run build`, `npm run lint` and TypeScript no-emit check passed on 17 September before subsequent dependency edits. Not a clean-checkout or current dependency validation. |
| CI-CHECK-001 — Hosted check execution | VALIDATED | [Run 35207328768](https://github.com/ugetmoresolutions-org/ugetmore_application_V2/actions/runs/35207328768), revision `50362b6`: clean install, lint, typecheck and build pass; tests/security fail and CI Gate fails accordingly. This validates failure detection, not application safety or mandatory merge enforcement. |

No payment, database isolation, deployment, restoration or production workflow is marked validated.

## 08. Pending work

| Unit | Status | Scope / completion evidence |
| --- | --- | --- |
| SEC-001 | IMPLEMENTED | PR #2 GET allowlists and server-only credentials integrated; offline supplier tests pass. Added error-log redaction tests. Live credential rotation and historical cleanup remain unverified; privileged cache/status endpoints require follow-up. |
| SEC-002 | PLANNED | HTML descriptions sanitized and token console logging removed. JWT/session verification and server-set HttpOnly cookie design still require the backend auth contract; F03 test remains red. |
| COM-001 | READY | Fix failed-checkout modal, response/error contract and retry/cancel behavior. Mock payment generation failure and confirm UI recovery. |
| COM-002 | PLANNED | Remove fabricated price/stock; unify cart/document totals; persist guest artwork durably; fix search fallback/cancellation. Cover boundary and reload cases. |
| DEP-001 | VALIDATED | Reconciled PR #2 package edits in CI worktree; clean npm ci PASS and fresh audit reports zero vulnerabilities. Hosted lint/typecheck/build PASS in run 36856476251. Local build has a machine-specific Google Fonts certificate-chain limitation. This is development validation, not release approval. Original checkout untouched. |
| API-001 | BLOCKED | Inspect backend identity, ownership, stock/pricing, order transactions and payment notification/idempotency controls. Needs backend source/specification and safe test environment. |
| QA-001 | PLANNED | Add regression tests and CI for security, commerce and integration failure paths; validate mobile and accessibility flows. |
| CI-001 | BLOCKED | Workflow execution verified; main rules active; existing defect remediation, real model review and independent approval remain outstanding. |
| OPS-001 | BLOCKED | Verify staging/production, monitoring, backups, restore and rollback. Needs runtime access, owners and operational evidence. |

Infrastructure expansion and broad refactoring are DEFERRED until requirements or measured bottlenecks justify them. READY means actionable scope, not that an owner has started it or that provider/account changes have been approved.

## 09. Dependency state

- SEC-001 code containment and COM-001 UI recovery can proceed independently of backend discovery. Use isolated fixtures instead of production supplier/payment mutations.
- Integrated SEC-002 and COM-002 approval require the relevant API-001 contracts. Release approval requires all applicable security, commerce, QA and operations gates.
- The previously uncommitted package edits were subsequently included in PR #2. CI integration reconciles them: Next 15.5.27, matching ESLint config, Flowbite React 0.12.17 with patched deepmerge, react-multi-carousel 2.8.5, removal of unused ExcelJS, official SheetJS 0.20.3 and patched PostCSS. Node 22 types match CI/Vitest. The lockfile was regenerated and clean installation passed; npm audit reports zero vulnerabilities on 1 October. See remediation decisions for compatibility tests and override rationale.
- The earlier audit found 26 affected package entries (1 critical, 19 high, 5 moderate, 1 low). This is historical evidence for the assessed dependency state; a new audit is required after reconciliation.

## 10. Decision register

| ID | Status | Decision / reason / impact |
| --- | --- | --- |
| DEC-001 | ACTIVE | Use root `PROJECT_STATE.md` as the single running snapshot, with dated reports for detailed evidence. Requested for collaborator handoff under AI-SWE-002A. |
| DEC-002 | ACTIVE | Publish this documentation independently of unrelated package edits, preserving their ownership and validation boundary. |
| DEC-003 | PROPOSED | Stabilize the existing Next.js/external-API architecture before broad redesign. Current evidence does not justify distributed infrastructure expansion. |
| DEC-004 | PROPOSED | Make the backend authoritative for identity, resource ownership, prices, stock, coupons and paid orders. Validate against actual backend implementation before choosing integration changes. |
| DEC-005 | ACTIVE | User requested AI-SWE-006 merge controls. Fail closed on required test/security/review failures; no automatic merge/deployment and no baseline suppression to obtain green CI. Main ruleset 23605355 is active with no bypass actors. |

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

F-identifiers map to the dated assessment. F04 HTML sanitization and F08 dependency remediation are locally validated; F01 current-source credentials and F02 catalog proxies are contained by PR #2. Historical credentials, supplier rotation, authentication and remaining findings stay open. The table below retains original impact and severity.

| Findings | Severity | Impact / affected approval |
| --- | --- | --- |
| F01–F02 | CRITICAL | Committed supplier credentials and unauthenticated credential-bearing proxy; block safe public exposure. Credential validity was not tested. |
| F03–F05 | HIGH | Unverified JWT authorization, unsafe description HTML and browser-readable token handling; block security approval. Backend compromise has not been demonstrated. |
| F06–F07 | HIGH | Checkout failure trap and invented fallback price/stock; block commerce approval. |
| F08 | CRITICAL | Historical dependency finding; current CI-branch audit reports zero vulnerabilities after reconciliation. Full application release acceptance remains outstanding. |
| F09–F11 | MEDIUM | Search fallback/cancellation, guest artwork persistence and misleading Amrod cache/status operations. |
| F12–F14 | MEDIUM | Shipping/document discrepancies, synthetic pre-payment receipts and inconsistent error contracts. |

## 14. Technical debt

- **DEBT-001 — DEFERRED:** large UI modules (branding details ~3,509 lines; coupons ~3,028), overlapping types and orchestration in components/hooks. Split around tested domain contracts after urgent correctness work.
- **DEBT-002 — PLANNED:** restore meaningful hook lint rules, simplify overlapping dependencies and document configuration. Target DEP-001/QA-001; avoid blanket suppression as a validation substitute.
- **DEBT-003 — DEFERRED:** define cache consistency and multi-instance behavior when workload/runtime requirements are known. Do not introduce shared infrastructure without a demonstrated need.

## 15. Validation state

| Area | Evidence / current limit |
| --- | --- |
| Build, lint, TypeScript | 1 October clean npm ci, lint and typecheck PASS. Local build blocked by SELF_SIGNED_CERT_IN_CHAIN when next/font fetches Google Fonts with TLS verification enabled. Hosted build, lint and typecheck PASS in run 36856476251 at cc01c4f. Workflow YAML and inline JavaScript parse PASS. |
| Unit/integration/E2E | Clean-install local result on 1 October: 26 PASS, 1 FAIL. Node suite: 7 pass, F03 fails; Vitest: 19 pass including supplier containment, secret-safe error logging, workbook formats and Flowbite themes. F04 is fixed. Live E2E NOT RUN. |
| Security | F04 sanitizer regression PASS; F03 fabricated admin cookie still FAIL. Current-source Gitleaks PASS; full history FAIL (one original Amrod credential and three synthetic fixture matches in PR #2). No supplier revocation or live session verification claimed. |
| Dependencies | Clean-install CI branch audit on 1 October: zero vulnerabilities; npm audit --audit-level=high PASS. Earlier 39 findings are historical. Runtime and live integration acceptance remain separate. |
| Database/payment | NOT VERIFIED: permissions, transactions, webhook validation, idempotency and reconciliation require backend review. |
| Performance | Bundle metrics only; product detail first-load JS ~280 kB. Browser/load capacity NOT TESTED. |
| Recovery/deployment | NOT VERIFIED. No restore, rollback or deployment acceptance performed. |
| Production readiness | BLOCKING. The full gate is in the assessment; no production-ready claim is supported. |

**Latest hosted evidence (1 October):** [PR run 36856476251](https://github.com/ugetmoresolutions-org/ugetmore_application_V2/actions/runs/36856476251) at `cc01c4f` completed. Lint, typecheck, build and dependency security PASS. Current-source secret scan PASS; history scan FAIL. Tests: 26 PASS, 1 FAIL (F03). AI review SKIPPED behind the secret gate; CI Gate FAIL and independent human approval still required. Later documentation-only commits do not change this tested application/workflow code.

**Historical review state (17 September, PR #1 revision `50362b6`):** hosted lint/typecheck/build PASS; tests FAIL (5 pass, 2 fail); dependency security FAIL; secret scanning FAIL (one supplier credential); AI engineering review SKIPPED because secret scanning failed; final CI Gate FAIL. Hosted run finished, it was not merely queued. Mocked review-control checks passed for clear findings, material findings, malformed output and unavailable service; real model inference remains unverified. Human review NOT APPROVED. Merge status BLOCKED / REVIEW_REQUIRED after server-side rule activation; admin access verified. No deployment or runtime approval is claimed.

Historical checks used an already modified lockfile/installed dependencies. Build tooling selected an ancestor lockfile and automatically rewrote a Flowbite CSS directive, later restored. The environment emitted a warning that Node TLS verification was disabled; that setting was not introduced by the assessment. Reproduce checks in a clean, correctly configured environment before release.

## 16. Environment & deployment state

- **Development:** local build previously completed; current dependency state needs revalidation.
- **Test/staging/production:** availability, access and configuration UNKNOWN; do not label absent or healthy without inspection.
- **CI/CD:** `Engineering checks` running on CI branch and PR #1; complete run evidence above. Initial activation was denied with write-only access. After admin access was granted, ruleset 23605355 was activated and effective main rules verified. Main requires PR approval and passing CI Gate with no bypass actors. The repository now resides under ugetmoresolutions-org. See [owner setup](docs/engineering-gates.md). Main does not contain these workflows until an approved merge; CODEOWNERS/Dependabot bootstrap remains pending.
- **Latest application deployment:** UNKNOWN. Pushing these documents is not an application deployment or proof that external auto-deployment succeeded.
- **Database, HTTPS, secrets, backups, restoration and rollback:** operational evidence outstanding.

## 17. Recent material changes

**2026-10-01 — PR #1 repair:** integrated PR #2 commits, reconciled Node/Vitest tests, sanitized descriptions, removed sensitive token and upstream-error logging, and repaired dependencies. See [remediation decisions](docs/pr1-remediation.md). Current-source Gitleaks passes. Historical scanning still reports the original credential plus three synthetic fixture matches from PR #2; no history was rewritten and no real credential suppressed.

**2026-09-17:** repository access verified; comprehensive assessment and local baseline checks completed; security/commerce findings recorded. Subsequent local manifest/lockfile changes observed with unknown ownership and no fresh validation. Added this state snapshot and linked assessment for GitHub collaboration; no application remediation is included in this documentation change.

**2026-09-17 — CI-001:** published PR #1 from isolated `codex/github-actions-gates`; hosted run validated lint/types/build and exposed known security failures. Required CI Gate name is reserved for PR/merge-group runs; ordinary pushes use Branch validation to avoid satisfying a PR requirement without AI review. Added one precise scanner false-positive exclusion for the reset-step enum; real supplier credentials remain detected. Original worktree package edits preserved. Initial activation was denied for the then write-only account. Subsequently admin access was verified, ruleset 23605355 activated, and effective main rules read back. PR #1 reports BLOCKED / REVIEW_REQUIRED; BLOCK-005 is resolved.

## 18. Blockers

| ID | Affected work | Required resolution |
| --- | --- | --- |
| BLOCK-001 | Security/release approval | Restrict exposed proxies and rotate/revoke committed supplier credentials through the account owner; validate replacement configuration. Repository code fixes alone do not revoke historical credentials. |
| BLOCK-002 | API-001; integrated auth/payment approval | Provide backend repository/specification and staging test setup; demonstrate ownership, price/stock enforcement and payment notification/idempotency behavior. |
| BLOCK-003 | OPS-001; production approval | Identify infrastructure owner and produce hosting, HTTPS, monitoring, backup/restore and rollback evidence. |
| BLOCK-004 — RESOLVED | Dependency reconciliation acceptance | Reconciled PR #2 dependency edits; clean install and zero-vulnerability audit verified; hosted lint, types and production build pass. Development checks do not imply release acceptance. |
| BLOCK-005 — RESOLVED | CI-001 mandatory merge enforcement | Ruleset 23605355 active; effective main rules require CI Gate and independent approval. Existing failing PR #1 reports BLOCKED / REVIEW_REQUIRED. No merge attempted. |
| BLOCK-006 | AI review/runtime review completion | GitHub Models access/quota and complete diff required. Secret scan must pass before source reaches model review; no skipped/unavailable review is treated as approval. |

## 19. Next action

**Current next action: obtain the backend JWT verification contract and supplier revocation confirmation.** PR #2 is integrated into PR #1; preserve the F03 failure and historical secret gate until their causes are resolved. Do not merge either PR or rewrite shared history to bypass required evidence.

**Transition condition:** implement backend-backed JWT verification, resolve the historical secret gate through a reviewed cleanup decision after supplier revocation, then obtain successful real model review and independent human approval. Application and production approval remain blocked.

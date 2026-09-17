# UGETMORE engineering review policy — AI-SWE-006

Changes enter `main` by pull request. No automatic merge or production deployment is configured by this change. Required checks and approvals must be enforced by the repository ruleset, not merely described here.

Review changed code, immediate dependencies and affected contracts using PROJECT_STATE.md and the relevant requirements. Prioritize security, session verification, ownership, correct totals/coupons/stock, payment idempotency, guest cart/artwork persistence, failures/retries/timeouts, API compatibility and deployment/data safety. Do not substitute style comments for engineering review or invent findings to fill a quota.

Every material finding records severity, confidence, file/line, defect, impact and correction. Track it in the PR discussion or an explicitly linked issue: OPEN → FIXED → RE-REVIEWED → RESOLVED, or investigate and document FALSE_POSITIVE. Do not include credential values or customer data.

| Severity | Merge treatment |
| --- | --- |
| CRITICAL | Block until fixed or independently demonstrated invalid. |
| HIGH | Block until fixed or independently demonstrated invalid. |
| MEDIUM | Block under this repository's initial conservative policy; correction/re-review required. |
| LOW | Advisory; reviewer may approve with a follow-up item. |

Severity and confidence are separate. Low-confidence material findings require investigation; they do not automatically disappear. CI does not implement label-based or author-controlled risk waivers. A false positive must be challenged with evidence, a regression test where applicable, and independent owner review. An exceptional change to enforcement itself is a separately reviewed policy change; do not disable gates to merge a feature.

At least one independent approving code-owner review is required for every PR. Dismiss stale approvals after new commits; require approval of the latest reviewable push and resolution of review conversations. Auth, payments, business totals, secrets, data access, migrations and CI changes always require explicit attention from the owner. Authors cannot approve their own PR.

The AI job uses GitHub Models with an ephemeral read-only token, no checkout and no execution tools. PR text is untrusted data. Findings are saved in the run summary with head/base SHAs. Service errors, missing diffs, invalid/truncated responses and review-budget overflow fail closed. Model review can miss defects: it supplements deterministic checks and human judgment. The owner must enable Models access/quota; unavailable review is not a passing review.

The test suite includes required negative security expectations for existing F03/F04 defects. These failures and the dependency/secret findings are real blockers. Do not skip tests, invert their assertions, suppress credentials, lower audit severity or add `continue-on-error` to make this baseline green. Rotate exposed credentials separately; a source edit alone cannot revoke historical secrets.

Passing CI is not release approval. Production changes additionally need staging validation, payment/backend verification, observable health/business metrics, a rollback procedure and an appropriate monitored rollout. Hosting and deployment details are still unverified; this workflow has no production credentials or deployment step.

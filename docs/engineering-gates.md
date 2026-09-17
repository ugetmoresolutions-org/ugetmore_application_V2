# Automated engineering gates

This implements the pre-merge portion of AI-SWE-006. It does not certify the existing application or deploy it.

## Checks on every branch push and pull request

`Engineering checks` runs on all branch pushes, PR updates/reopens, merge groups and manual dispatch. It has no path filters, so documentation-only changes also receive checks. Four isolated jobs run lint, TypeScript, tests and the production build using Node 22 and `npm ci --ignore-scripts`. The lockfile is authoritative; no floating install or automatic dependency fix occurs in CI.

Separate jobs audit dependencies at high/critical severity and scan secrets with Gitleaks (current files plus fetched history, redacted output). Gitleaks is version-pinned and its archive SHA-256 verified. GitHub Actions are pinned to full commit SHAs. Tokens are read-only, checkout does not persist credentials, jobs have time limits, and outdated runs are cancelled. No `pull_request_target`, self-hosted runner, production secret or automatic merge/deploy is used.

On PRs, independent contextual review uses GitHub Models `openai/gpt-4.1` through the ephemeral `GITHUB_TOKEN` with `models: read`. No separate paid API key is configured. The owner must ensure Models is enabled with adequate quota; this setup does not purchase quota or enable billing. The job reads the base policy/state and changed text via GitHub's API, executes no PR code, records findings in the Actions summary and fails on material findings or incomplete review. Lockfile contents and binary assets are excluded from model review; scanners and human review still apply. Large/missing textual diffs fail rather than silently approving partial review; split oversized PRs.

The final **CI Gate** job requires every deterministic job to succeed and, on PRs, the AI review to succeed. Failed, cancelled or unexpectedly skipped prerequisites cannot yield a green gate. Push/merge-group checks run deterministic validation; the pull-request review requirement remains separately enforced by the main ruleset.

## Test coverage and current red baseline

`npm test` uses Node's test runner and the installed TypeScript compiler to execute actual source with explicit dependency doubles. Tests cover branding tier boundaries, multi-color setup/design fees, guest-cart no-op and failure recovery, plus negative authentication/HTML expectations. They require no supplier/backend account or live payment. This is a starter regression suite, not complete E2E coverage.

The known F03 and F04 security expectations must currently fail. The committed dependency baseline also contains advisories, and committed credentials must be detected. CI being red on this baseline is correct enforcement. Resolving these issues is a separate remediation task; this PR does not silently weaken gates to obtain a green result. The owner's existing uncommitted dependency edits are excluded from this branch.

Run locally:

```sh
npm ci --ignore-scripts --no-audit --no-fund
npm run lint
npm run typecheck
npm run test:unit
npm run test:security
npm run build
npm audit --audit-level=high
```

## Required owner setup — this is what blocks merging

At setup time `Mnqobi-Developer` has repository **write**, not admin, permission. `ugetmoresolutions` is the current admin. Workflow failures alone cannot prevent a collaborator from merging or pushing to an unprotected branch.

The owner should review the PR and [.github/main-ruleset.json](../.github/main-ruleset.json). After checking for an existing equivalent rule, use an owner-authenticated GitHub CLI from this branch:

```sh
gh api --method POST repos/ugetmoresolutions/ugetmore_application_V2/rulesets --input .github/main-ruleset.json
gh api repos/ugetmoresolutions/ugetmore_application_V2/rulesets
gh api repos/ugetmoresolutions/ugetmore_application_V2/rules/branches/main
```

Create once; update an existing rule instead of duplicating it. The rule targets main, has no bypass actors, requires a PR, one approving code-owner review, approval of the latest push, dismissed stale approvals, resolved threads and an up-to-date successful **CI Gate** from GitHub Actions (app ID 15368). It blocks branch deletion and force pushes. All paths require the current admin code owner; add additional qualified owners to avoid a single-reviewer bottleneck. A PR authored by the only code owner will need another eligible code owner.

Branch rules are server-side configuration: committing JSON does **not** activate them. Keep this distinction visible in PROJECT_STATE.md. Once active, verify with a disposable PR that a failing check blocks merging; do not test by pushing to main. The initial PR is deliberately unmerged while critical baseline failures remain. Remediate them on reviewed branches; do not bypass the gate for bootstrap convenience. Until the owner installs rules, direct pushes remain technically possible and must not be described as prohibited by GitHub.

After workflows reach main, CODEOWNERS and Dependabot operate from the default branch. Bootstrap owner review must be requested/obtained explicitly because CODEOWNERS is not yet on main. No independent approval is claimed by this setup.

## Finding and release policy

See [the project review policy](../.github/ENGINEERING_REVIEW.md). Critical/high/medium findings block; low findings are advisory. No label-based exception bypass exists. Record disputed findings and evidence; independent review must establish a false positive before changing the gate or affected code. Existing secrets in history require rotation and a separately reviewed historical-remediation decision, not an unchecked scanner allowlist.

There is no production deployment workflow yet. Runtime monitoring, staged rollout, rollback and backend/payment verification remain prerequisites for release. Review Actions logs/summaries for the exact head SHA and keep PROJECT_STATE.md current after meaningful validation.

References: [GitHub protected branches](https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-protected-branches/about-protected-branches), [secure Actions use](https://docs.github.com/en/actions/reference/security/secure-use), and [GitHub Models in workflows](https://docs.github.com/en/github-models/integrating-ai-models-into-your-development-workflow).

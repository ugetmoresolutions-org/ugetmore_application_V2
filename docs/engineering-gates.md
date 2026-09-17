# Automated engineering gates

This implements the pre-merge portion of AI-SWE-006. It does not certify the existing application or deploy it.

## Checks on every branch push and pull request

`Engineering checks` runs on all branch pushes, PR updates/reopens, merge groups and manual dispatch. It has no path filters, so documentation-only changes also receive checks. Four isolated jobs run lint, TypeScript, tests and the production build using Node 22 and `npm ci --ignore-scripts`. The lockfile is authoritative; no floating install or automatic dependency fix occurs in CI.

Separate jobs audit dependencies at high/critical severity and scan secrets with Gitleaks (current files plus fetched history, redacted output). Gitleaks is version-pinned and its archive SHA-256 verified. GitHub Actions are pinned to full commit SHAs. Tokens are read-only, checkout does not persist credentials, jobs have time limits, and outdated runs are cancelled. No `pull_request_target`, self-hosted runner, production secret or automatic merge/deploy is used.

On PRs, independent contextual review uses GitHub Models `openai/gpt-4.1` through the ephemeral `GITHUB_TOKEN` with `models: read`. No separate paid API key is configured. The owner must ensure Models is enabled with adequate quota; this setup does not purchase quota or enable billing. The job reads the base policy/state and changed text via GitHub's API, executes no PR code, records findings in the Actions summary and fails on material findings or incomplete review. Lockfile contents and binary assets are excluded from model review; scanners and human review still apply. Large/missing textual diffs fail rather than silently approving partial review; split oversized PRs.

The final **CI Gate** job requires every deterministic job to succeed and, on PRs, the AI review to succeed. Failed, cancelled or unexpectedly skipped prerequisites cannot yield a green gate. Push/manual runs use the different name **Branch validation**, so a successful push without AI review cannot satisfy the required PR context. Merge-group checks revalidate deterministic behavior after queue integration; the pull-request review requirement remains separately enforced by the main ruleset.

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

## Active mandatory merge rules

On 17 September 2026, admin access for `Mnqobi-Developer` was verified and [ruleset 23605355](https://github.com/ugetmoresolutions-org/ugetmore_application_V2/rules/23605355) was activated. The repository moved to `ugetmoresolutions-org/ugetmore_application_V2`. The earlier write-only permission blocker is resolved.

Effective rules for main were read back through GitHub's API. PR #1 reports **BLOCKED / REVIEW_REQUIRED**. No merge was attempted. The rule has no bypass actors and requires a PR, one approving review, code-owner review where applicable, approval of the latest push by someone other than its pusher, dismissed stale approvals, resolved threads and an up-to-date successful **CI Gate** from GitHub Actions (app ID 15368). Branch deletion and force pushes are blocked.

The committed [.github/main-ruleset.json](../.github/main-ruleset.json) is the configuration template; the live server-side rule provides enforcement. Inspect it with:

```sh
gh api repos/ugetmoresolutions-org/ugetmore_application_V2/rulesets/23605355
gh api repos/ugetmoresolutions-org/ugetmore_application_V2/rules/branches/main
```

Update the existing rule instead of creating duplicates. The initial PR remains unmerged while critical baseline failures remain. Remediate them on reviewed branches without bypassing the gate. Ensure configured code owners retain write access after the repository transfer; add qualified owners to avoid a single-reviewer bottleneck.

After workflows reach main, CODEOWNERS and Dependabot operate from the default branch. Bootstrap owner review must be requested/obtained explicitly because CODEOWNERS is not yet on main. No independent approval is claimed by this setup.

## Finding and release policy

Hosted validation on 17 September 2026: [PR run 35207328768](https://github.com/ugetmoresolutions-org/ugetmore_application_V2/actions/runs/35207328768) at `50362b6` passed lint, typecheck and build. Tests reported five passing and two failing security expectations; dependency and secret scans failed; AI review was skipped after secret-scan failure; CI Gate failed. This demonstrates that known failures propagate, not application safety. Mandatory main rules were subsequently activated and verified as described above. Real Models inference is still unverified; mocked control-flow checks covered clean findings, material findings, malformed output and service denial.

See [the project review policy](../.github/ENGINEERING_REVIEW.md). Critical/high/medium findings block; low findings are advisory. No label-based exception bypass exists. Record disputed findings and evidence; independent review must establish a false positive before changing the gate or affected code. Existing secrets in history require rotation and a separately reviewed historical-remediation decision, not an unchecked scanner allowlist.

There is no production deployment workflow yet. Runtime monitoring, staged rollout, rollback and backend/payment verification remain prerequisites for release. Review Actions logs/summaries for the exact head SHA and keep PROJECT_STATE.md current after meaningful validation.

References: [GitHub protected branches](https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-protected-branches/about-protected-branches), [secure Actions use](https://docs.github.com/en/actions/reference/security/secure-use), and [GitHub Models in workflows](https://docs.github.com/en/github-models/integrating-ai-models-into-your-development-workflow).

# n8n automation

## Scope and repository audit

This repository contains Python/FastAPI services, Python model-training and evaluation scripts, a C# component, Docker support, GitHub Actions, pytest-based tests, and benchmark workflows. It is an ML repository: model evidence must remain tied to committed artifacts and no workflow may manufacture or overwrite benchmark values.

No existing `n8n/` directory or n8n workflow definition was found. Existing GitHub Actions are preserved; this package does not replace CI, model evaluation, or deployment workflows.

## Prerequisites

Import the JSON files from `n8n/` into a self-hosted n8n instance, then attach one organization-wide **GitHub OAuth2** credential to every GitHub/GitHub Trigger node.

Required environment variables:

- `N8N_BASE_URL`: base URL of the self-hosted n8n instance. It is not stored in this repository.
- `GITHUB_OWNER=CoreyLeath-code`.
- `GITHUB_TOKEN`: optional token for HTTP Request-node fallbacks. Prefer the n8n credential store and do not put this value in workflow JSON.

The workflow definitions are inactive after import. Activate them only after the credential is bound and the repository webhook registration shown by n8n is reachable through `N8N_BASE_URL`.

## Workflows included

| File | Trigger | Actions | Manual execution and recovery |
|---|---|---|---|
| `n8n/label-bootstrap.json` | Manual | Creates only missing issue labels from the approved taxonomy; it never edits an existing label. | Run once after binding the OAuth credential. If it fails, correct the credential permissions and rerun; do not delete or recolor existing labels. |
| `n8n/issue-triage.json` | GitHub `issues` event | On newly opened issues, classifies obvious bug/feature/documentation/question keywords, adds `needs-triage` plus a conservative priority label, and leaves ambiguous classification for a maintainer. | Use n8n test mode with a redacted issue payload. If label writes fail, run label bootstrap and retry the execution. |
| `n8n/pull-request-assistant.json` | GitHub `pull_request` event | On opened or ready-for-review PRs, posts a review checklist covering tests, affected modules, documentation, and linked issues. It does not approve, merge, or modify code. | Test against a draft PR. Failed comments can be retried after restoring GitHub credential access. |

## Approved label taxonomy

The bootstrap workflow manages only these labels when absent:

- `bug`, `feature`, `documentation`, `enhancement`, `question`, `needs-triage`
- `priority:high`, `priority:medium`, `priority:low`
- `good first issue`, `help wanted`

It must not modify a label that already exists, including its color.

## Workflows deliberately skipped

| Requested capability | Decision | Evidence |
|---|---|---|
| Repository health report | Skipped | Writing `docs/reports/repository-health.md` from n8n would require an approved automated commit/branch policy. None is documented. |
| Release assistant | Skipped | No verified release trigger and release-artifact policy was identified during this audit. |
| README consistency issue creation | Skipped | The README contains many historic, environment-specific claims and links. Automatic issue creation would create noisy findings until a repository-specific policy is agreed. |
| Documentation reminder | Skipped | No reliable definition of “changes significantly” is committed. |
| Model evidence automation | Skipped | The repository has model/benchmark scripts, but the benchmark workflows contain simulated DVC synchronization and no safe n8n data/model-access contract. |
| Security monitoring | Skipped | Existing GitHub Actions provide security/quality automation; no documented alert-routing or issue-deduplication policy exists. |
| Performance monitoring | Skipped | `benchmarks.yml` and `mlops-benchmarks.yml` run only for selected pull-request paths and do not support manual dispatch. n8n must not fabricate benchmark runs or values. |
| Portfolio dashboard | Skipped | This is organization-wide, not a repository-local artifact; no canonical dashboard repository or ownership policy is documented. |
| GitHub Project sync | Skipped | No GitHub Project configuration was found. |
| Community and reminder automation | Skipped | No contribution/discussion policy or notification destination is documented. The included PR assistant is deliberately limited to a neutral checklist. |

## Security and recovery

Use least-privilege GitHub OAuth scopes sufficient for webhook registration, issue labels/comments, and PR comments. Store credentials only in n8n. Review failed executions in n8n, correct the credential or GitHub webhook reachability, and retry the recorded execution. Never paste tokens, webhook URLs, model data, benchmark outputs, or production telemetry into the workflow definitions.

## Validation boundary

The files are valid JSON and contain no credentials, tokens, or n8n URL. They cannot be activated or integration-tested until a reachable self-hosted n8n instance and the organization-wide OAuth credential are supplied.

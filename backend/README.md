# UGetMore Backend

ASP.NET Core API replacing the external "business API" the Next.js frontend currently calls at
`${NEXT_PUBLIC_URL}/api` — see `C:\Users\dntsi\.claude\plans\eager-tumbling-taco.md` for the full
rebuild plan (schema, phasing, auth design, deferred items).

## Status: Phase 1 (foundation)

- `src/Domain` — `Product`, `ProductVariant`, `SupplierSku`, `Category`, `Stock`, `BrandingOption`/
  `BrandingPriceTier`. These collapse the 7-8 overlapping "Product" interfaces and 3 conflicting
  `Category` shapes found in the frontend into one canonical graph (see the plan, §2).
- `src/Application` / `src/Infrastructure` — a thin read-only catalog service backing the first
  three endpoints.
- `src/Api` — `GET /api/categories`, `GET /api/products`, `GET /api/products/{id}`, plus
  `/health` (liveness, no DB dependency) and `/health/ready` (checks Postgres).
- Not yet built: auth (Entra External ID / BFF), cart/order/payment endpoints, branding/school
  modules — these land in Phases 2-5 per the plan.

## Running locally

Requires the .NET 8 SDK and Docker (for Postgres).

```bash
cd backend
docker compose up -d          # starts local Postgres on 5432 (see docker-compose.yml)
dotnet ef database update --project src/Infrastructure --startup-project src/Api
dotnet run --project src/Api
```

Swagger UI is available at `/swagger` in Development. `appsettings.Development.json` points at the
docker-compose Postgres instance with a placeholder password (`local-dev-only`) — this is not a real
secret and is safe to commit; it only works against a container no one else can reach.

**Never put real credentials in `appsettings*.json`.** Production connection strings, PayFast
credentials, and Entra secrets come from Azure Key Vault via managed identity once deployed, or from
.NET User Secrets (`dotnet user-secrets set "ConnectionStrings:Postgres" "..."`) for a developer who
needs to point at something other than the local Postgres container.

## Tests

```bash
dotnet test
```

- `tests/Domain.Tests` — pure domain logic (e.g. `Stock`'s reserve/release/commit guards), no DB
  required.
- `tests/Api.IntegrationTests` — `WebApplicationFactory`-based. The current `/health` liveness test
  needs no database; tests exercising `/health/ready` or the catalog endpoints will need
  Testcontainers-Postgres (Docker) once added — these run in CI (`.github/workflows/ci.yml`'s
  `backend` job) where Docker is available, even though Docker isn't available in every local dev
  environment.

## Adding a migration

```bash
dotnet ef migrations add <Name> --project src/Infrastructure --startup-project src/Api --output-dir Persistence/Migrations
```

This only needs the EF Core model, not a reachable database — safe to run without Docker.

## What's deliberately not here yet

See the plan's §7 (Explicit deferrals) and §5 (Phases 2-5) — newsletter campaigns, business
multi-user permissions, Redis, and the branding/school/order/payment modules are scoped for later
phases, not omissions.

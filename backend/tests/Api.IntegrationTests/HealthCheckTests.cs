using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace UGetMore.Api.IntegrationTests;

// Only exercises the liveness endpoint, which is deliberately DB-independent (Program.cs: /health
// runs no checks). Readiness (/health/ready) and the catalog endpoints need a real Postgres instance
// (Testcontainers in CI) and are added once that's wired up — see backend/README.md.
public class HealthCheckTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public HealthCheckTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Liveness_ReturnsHealthy_WithoutADatabase()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}

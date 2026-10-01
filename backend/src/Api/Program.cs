using UGetMore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Real HTTP status codes + one structured ProblemDetails body on every failure — replacing the four
// incompatible error shapes the frontend's four HTTP clients currently produce (see plan §3).
builder.Services.AddProblemDetails();

builder.Services.AddInfrastructure(builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString!, name: "postgres", tags: new[] { "ready" });

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        var allowedOrigin = builder.Configuration["Cors:FrontendOrigin"];
        if (!string.IsNullOrWhiteSpace(allowedOrigin))
        {
            policy.WithOrigins(allowedOrigin).AllowCredentials().AllowAnyHeader().AllowAnyMethod();
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseHttpsRedirection();
app.UseCors("Frontend");

app.MapControllers();

// Liveness (/health): process is responsive — deliberately runs no dependency checks, so it can't be
// dragged down by a slow/unreachable database. Readiness (/health/ready): dependencies (Postgres) are
// actually reachable — this is the one deployment should gate traffic/rollout on, not liveness.
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => false,
});
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
});

app.Run();

// Exposed for WebApplicationFactory-based integration tests.
public partial class Program { }

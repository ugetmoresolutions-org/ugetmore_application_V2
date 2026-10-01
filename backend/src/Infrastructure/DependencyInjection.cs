using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UGetMore.Application.Catalog;
using UGetMore.Infrastructure.Catalog;
using UGetMore.Infrastructure.Persistence;

namespace UGetMore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Postgres")
            ?? throw new InvalidOperationException(
                "Missing ConnectionStrings:Postgres. Set it via appsettings, an environment variable " +
                "(ConnectionStrings__Postgres), or (in Azure) a Key Vault reference — never commit a real one.");

        services.AddDbContext<UGetMoreDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IProductCatalogService, ProductCatalogService>();

        return services;
    }
}

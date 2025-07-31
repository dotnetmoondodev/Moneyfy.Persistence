namespace Persistence;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

internal static partial class CommonDependencies
{
    internal static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        string dbConnUrl )
    {
        services.AddHealthChecks().AddSqlServer(
            dbConnUrl,
            Constants.HealthCheckSettings.Query,
            null,
            Constants.HealthCheckSettings.Name,
            HealthStatus.Unhealthy,
            Constants.HealthCheckSettings.Tags,
            TimeSpan.FromSeconds( Constants.HealthCheckSettings.TimeoutSeconds ) );

        services.AddMemoryCache();

        return services;
    }
}
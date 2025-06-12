using Application;
using Application.Payments;
using Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Persistence.Payments;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration )
    {
        var connectionString = configuration.GetValue<string>( ApiSettings.DBConnection ) ??
            throw new InvalidOperationException( $"Connection string '{ApiSettings.DBConnection}' is not configured." );

        services.AddDbContext<PaymentsDbContext>( options =>
            options.UseSqlServer( connectionString,
                opt => opt.MigrationsAssembly( Constants.ThisAssemblyName ) ) );

        services.AddScoped<IAppDbContext>( provider => provider.GetRequiredService<PaymentsDbContext>() );
        services.AddScoped<IPaymentsRepository, PaymentsRepository>();

        services.AddHealthChecks().AddSqlServer(
            connectionString,
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

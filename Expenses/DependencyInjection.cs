using Application.Expenses;
using Domain.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Persistence.Expenses;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration )
    {
        var connectionString = configuration.GetConnectionString( Constants.AppSettings.DBConnName ) ??
            throw new InvalidOperationException( $"Connection string '{Constants.AppSettings.DBConnName}' is not configured." );

        services.AddDbContext<ExpensesDbContext>( options =>
            options.UseSqlServer( connectionString,
                opt => opt.MigrationsAssembly( Constants.ThisAssemblyName ) ) );

        services.AddScoped<IAppDbContext>( provider => provider.GetRequiredService<ExpensesDbContext>() );
        services.AddScoped<IExpensesRepository, ExpensesRepository>();

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

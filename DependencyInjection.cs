using Application.Abstractions;
using Domain.Expenses;
using Domain.Incomes;
using Domain.Notifications;
using Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Persistence.Repositories;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration )
    {
        var connectionString = configuration.GetConnectionString( Constants.DatabaseConnName ) ??
            throw new InvalidOperationException( $"Connection string '{Constants.DatabaseConnName}' is not configured." );

        services.AddDbContext<AppDbContext>( options =>
            options.UseSqlServer( connectionString,
                opt => opt.MigrationsAssembly( "Persistence" ) ) );

        services.AddScoped<IAppDbContext>( provider => provider.GetRequiredService<AppDbContext>() );
        services.AddScoped<IExpensesRepository, ExpensesRepository>();
        services.AddScoped<IIncomesRepository, IncomesRepository>();
        services.AddScoped<IPaymentsRepository, PaymentsRepository>();
        services.AddScoped<INotificationsRepository, NotificationsRepository>();

        services.AddHealthChecks().AddSqlServer(
            connectionString,
            "select 1;",
            null,
            "Sql-Server",
            HealthStatus.Unhealthy,
            ["ready"],
            TimeSpan.FromSeconds( 5 ) );

        services.AddMemoryCache();

        return services;
    }
}

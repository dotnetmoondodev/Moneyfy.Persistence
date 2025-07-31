namespace Persistence.Expenses;

using Application.Expenses;
using Domain;
using Domain.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfigurationBuilder configuration,
        IHostEnvironment hostEnvironment )
    {
        services.AddCommonServices( configuration, hostEnvironment, out var dbConnStr )
            .AddDbContext<ExpensesDbContext>( options =>
                options.UseSqlServer( dbConnStr, opt => opt.MigrationsAssembly( Constants.ThisAssemblyName ) ) );

        services.AddScoped<IAppDbContext>( provider => provider.GetRequiredService<ExpensesDbContext>() );
        services.AddScoped<IRepository<Expense>, ExpensesRepository>();

        return services;
    }
}

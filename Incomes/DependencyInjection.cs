namespace Persistence.Incomes;

using Application.Incomes;
using Domain;
using Domain.Incomes;
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
            .AddDbContext<IncomesDbContext>( options =>
                options.UseSqlServer( dbConnStr, opt => opt.MigrationsAssembly( Constants.ThisAssemblyName ) ) );

        services.AddScoped<IAppDbContext>( provider => provider.GetRequiredService<IncomesDbContext>() );
        services.AddScoped<IRepository<Income>, IncomesRepository>();

        return services;
    }
}

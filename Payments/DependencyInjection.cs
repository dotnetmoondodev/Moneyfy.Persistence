namespace Persistence.Payments;

using Application.Payments;
using Domain;
using Domain.Payments;
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
        services.AddCommonServices( configuration, hostEnvironment, out var dbConnStr );

        services.AddDbContext<PaymentsDbContext>( options =>
            options.UseSqlServer( dbConnStr, opt =>
                opt.MigrationsAssembly( Constants.ThisAssemblyName ) ) );

        services.AddScoped<IAppDbContext>( provider =>
            provider.GetRequiredService<PaymentsDbContext>() );

        services.AddScoped<IRepository<Payment>, PaymentsRepository>();

        return services;
    }
}

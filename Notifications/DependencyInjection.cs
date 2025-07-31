namespace Persistence.Notifications;

using Application.Notifications;
using Domain;
using Domain.Notifications;
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

        services.AddDbContext<NotificationsDbContext>( options =>
            options.UseSqlServer( dbConnStr, opt =>
                opt.MigrationsAssembly( Constants.ThisAssemblyName ) ) );

        services.AddScoped<IAppDbContext>( provider =>
            provider.GetRequiredService<NotificationsDbContext>() );

        services.AddScoped<IRepository<Notification>, NotificationsRepository>();

        return services;
    }
}

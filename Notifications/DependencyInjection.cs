namespace Persistence.Notifications;

using Domain.Notifications;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfigurationBuilder configuration,
        bool isProduction )
    {
        services.AddCommonServices( configuration, isProduction );
        services.AddSingleton<IRepository<Notification>>( provider =>
        {
            var database = provider.GetService<IMongoDatabase>();
            return new NotificationsRepository( database! );
        } );

        return services;
    }
}

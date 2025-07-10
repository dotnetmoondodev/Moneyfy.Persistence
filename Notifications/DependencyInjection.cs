using Domain.Notifications;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Persistence.Notifications;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfigurationBuilder configuration,
        bool isProduction )
    {
        services.AddPersistenceCommonServices( configuration, isProduction );
        services.AddSingleton<IRepository<Notification>>( provider =>
        {
            var database = provider.GetService<IMongoDatabase>();
            return new NotificationsRepository( database! );
        } );

        return services;
    }
}

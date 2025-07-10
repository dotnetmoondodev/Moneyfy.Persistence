using Domain.Payments;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Persistence.Payments;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfigurationBuilder configuration,
        bool isProduction )
    {
        services.AddPersistenceCommonServices( configuration, isProduction );
        services.AddSingleton<IRepository<Payment>>( provider =>
        {
            var database = provider.GetService<IMongoDatabase>();
            return new PaymentsRepository( database! );
        } );

        return services;
    }
}

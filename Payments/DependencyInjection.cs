namespace Persistence.Payments;

using Domain.Payments;
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
        services.AddSingleton<IRepository<Payment>>( provider =>
        {
            var database = provider.GetService<IMongoDatabase>();
            return new PaymentsRepository( database! );
        } );

        return services;
    }
}

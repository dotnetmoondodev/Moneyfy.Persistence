using Domain.Incomes;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Persistence.Incomes;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfigurationBuilder configuration,
        bool isProduction )
    {
        services.AddPersistenceCommonServices( configuration, isProduction );
        services.AddSingleton<IRepository<Income>>( provider =>
        {
            var database = provider.GetService<IMongoDatabase>();
            return new IncomesRepository( database! );
        } );

        return services;
    }
}

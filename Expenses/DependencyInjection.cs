using Domain;
using Domain.Expenses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Persistence.Expenses;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfigurationBuilder configuration,
        bool isProduction )
    {
        services.AddPersistenceCommonServices( configuration, isProduction );
        services.AddSingleton<IRepository<Expense>>( provider =>
        {
            var database = provider.GetService<IMongoDatabase>();
            return new ExpensesRepository( database! );
        } );

        return services;
    }
}

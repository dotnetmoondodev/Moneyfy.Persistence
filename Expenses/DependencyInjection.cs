using Application;
using Application.Expenses;
using Domain.Expenses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Persistence.Expenses;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(
        this IServiceCollection services,
        IConfiguration configuration )
    {
        BsonSerializer.RegisterSerializer( new GuidSerializer( BsonType.String ) );
        BsonSerializer.RegisterSerializer( new DateTimeSerializer( BsonType.String ) );

        var connectionString = configuration.GetValue<string>( ApiSettings.DBConnection ) ??
            throw new InvalidOperationException( $"Connection string '{ApiSettings.DBConnection}' is not configured." );

        services.AddSingleton( serviceProvider =>
        {
            var mongoClient = new MongoClient( connectionString );
            return mongoClient.GetDatabase( nameof( ApiEndpoints.Expenses ) );
        } );

        services.AddSingleton<IExpensesRepository>( provider =>
        {
            var database = provider.GetService<IMongoDatabase>();
            return new ExpensesRepository( database! );
        } );

        services.AddHealthChecks().AddMongoDb( sp =>
            sp.GetService<IMongoDatabase>()!,
                name: Constants.HealthCheckSettings.Name,
                timeout: TimeSpan.FromSeconds( Constants.HealthCheckSettings.TimeoutSeconds ),
                tags: Constants.HealthCheckSettings.Tags );

        services.AddMemoryCache();
        return services;
    }
}

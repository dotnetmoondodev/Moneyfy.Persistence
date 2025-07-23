namespace Persistence;

using Application.Settings;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

internal static partial class CommonDependencies
{
    internal static IServiceCollection AddMongoDbServices(
        this IServiceCollection services,
        WebApiSettings settings )
    {
        BsonSerializer.RegisterSerializer( new GuidSerializer( BsonType.String ) );
        BsonSerializer.RegisterSerializer( new DateTimeSerializer( BsonType.String ) );

        services.AddSingleton( serviceProvider =>
        {
            var mongoClient = new MongoClient( settings.DBConnection );
            return mongoClient.GetDatabase( Constants.DatabaseName );
        } );

        services.AddHealthChecks().AddMongoDb( sp =>
            sp.GetService<IMongoDatabase>()!,
                name: Constants.HealthCheckSettings.Name,
                timeout: TimeSpan.FromSeconds( Constants.HealthCheckSettings.TimeoutSeconds ),
                tags: Constants.HealthCheckSettings.Tags );

        return services;
    }
}
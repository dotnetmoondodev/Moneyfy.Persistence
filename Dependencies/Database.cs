namespace Persistence;

using Application.Settings;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;

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
            /* We include here the OpenTelemetry Instrumentation for MongoDB */
            var clientSettings = MongoClientSettings.FromUrl( new MongoUrl( settings.DBConnection ) );
            var options = new InstrumentationOptions { CaptureCommandText = true };
            clientSettings.ClusterConfigurator = cb => cb.Subscribe( new DiagnosticsActivityEventSubscriber( options ) );

            var mongoClient = new MongoClient( clientSettings );
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
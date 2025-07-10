using Application;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Persistence;

public static class CommonDependencies
{
    public static IServiceCollection AddPersistenceCommonServices(
        this IServiceCollection services,
        IConfigurationBuilder configBuilder,
        bool isProduction )
    {
        if ( isProduction )
        {
            var keyName = configBuilder.Build().GetValue<string>( ApiSettings.KeyVaultName );
            configBuilder.AddAzureKeyVault(
                 new Uri( $"https://{keyName}.vault.azure.net/" ),
                 new DefaultAzureCredential()
            );
        }
        else
        {
            configBuilder.AddEnvironmentVariables();
        }

        var config = configBuilder.Build();

        var logLevel = config.GetValue<string>( LoggerSettings.Default );
        var environment = isProduction ? "PRD" : "DEV";

        Console.WriteLine( $"Current LogLevel: {logLevel}( {environment} )" );

        services.AddLogging( loggingBuilder =>
            loggingBuilder.SetMinimumLevel( LoggerSettings.GetLogLevel( logLevel ) ).AddConsole() );

        BsonSerializer.RegisterSerializer( new GuidSerializer( BsonType.String ) );
        BsonSerializer.RegisterSerializer( new DateTimeSerializer( BsonType.String ) );

        var connectionString = config.GetValue<string>( ApiSettings.DBConnection ) ??
            throw new InvalidOperationException( $"Connection string '{ApiSettings.DBConnection}' is not configured." );

        services.AddSingleton( serviceProvider =>
        {
            var mongoClient = new MongoClient( connectionString );
            return mongoClient.GetDatabase( Constants.DatabaseName );
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
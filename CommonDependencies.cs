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
        configBuilder.AddEnvironmentVariables();
        var config = configBuilder.Build();

        /* To work locally with Docker this value should be Empty */
        var keyVault = config.GetValue<string>( ApiSettings.KeyVaultName );

        if ( isProduction )
        {
            if ( !string.IsNullOrEmpty( keyVault!.Trim() ) )
            {
                configBuilder.AddAzureKeyVault(
                     new Uri( $"https://{keyVault}.vault.azure.net/" ),
                     new DefaultAzureCredential()
                );

                config = configBuilder.Build();
            }
        }

        var logLevel = config.GetValue<string>( LoggerSettings.Default );
        var environment = isProduction ? "PRD" : "DEV";

        Console.WriteLine( $"LogLevel=[{logLevel}], Env=[{environment}], Key=[{keyVault}]" );
        services.AddLogging( loggingBuilder =>
            loggingBuilder.SetMinimumLevel( LoggerSettings.GetLogLevel( logLevel ) ).AddConsole() );

        /* Important validations to make here before continue */
        var settingsValue = config.GetValue<string>( ApiSettings.Authority );
        ArgumentNullException.ThrowIfNullOrEmpty( settingsValue!.Trim(), nameof( ApiSettings.Authority ) );
        Console.WriteLine( $"{nameof( ApiSettings.Authority )}=[{ApiSettings.MaskStrValue( settingsValue )}]" );

        settingsValue = config.GetValue<string>( ApiSettings.Audience );
        ArgumentNullException.ThrowIfNullOrEmpty( settingsValue!.Trim(), nameof( ApiSettings.Audience ) );
        Console.WriteLine( $"{nameof( ApiSettings.Audience )}=[{ApiSettings.MaskStrValue( settingsValue )}]" );

        settingsValue = config.GetValue<string>( ApiSettings.DBConnection );
        ArgumentNullException.ThrowIfNullOrEmpty( settingsValue!.Trim(), nameof( ApiSettings.DBConnection ) );
        Console.WriteLine( $"{nameof( ApiSettings.DBConnection )}=[{ApiSettings.MaskStrValue( settingsValue )}]" );

        BsonSerializer.RegisterSerializer( new GuidSerializer( BsonType.String ) );
        BsonSerializer.RegisterSerializer( new DateTimeSerializer( BsonType.String ) );

        services.AddSingleton( serviceProvider =>
        {
            var mongoClient = new MongoClient( settingsValue );
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
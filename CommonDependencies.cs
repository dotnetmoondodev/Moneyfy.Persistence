using Application.Settings;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.Configuration;
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
        var settings = config.GetSection( nameof( WebApiSettings ) ).Get<WebApiSettings>();

        if ( isProduction )
        {
            /* To work locally with Docker this value should be Empty */
            if ( !string.IsNullOrEmpty( settings!.KeyVaultName!.Trim() ) )
            {
                configBuilder.AddAzureKeyVault(
                     new Uri( $"https://{settings.KeyVaultName}.vault.azure.net/" ),
                     new DefaultAzureCredential()
                );

                config = configBuilder.Build();
                settings = config.GetSection( nameof( WebApiSettings ) ).Get<WebApiSettings>();
            }
        }

        var environment = isProduction ? "Production" : "Development";
        Console.WriteLine( $"Environment( {environment} ), KeyVaultName( {settings!.KeyVaultName} )" );

        /* Important validations to make here before continue */
        if ( settings is null || !settings.DataIsValid() )
            throw new InvalidConfigurationException( $"Configuration values: {nameof( WebApiSettings )} aren't defined or invalid." );

        services.AddLogging( loggingBuilder => loggingBuilder.AddSeq( settings.SeqServerUrl ) );

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

        services.AddMemoryCache();
        return services;
    }
}
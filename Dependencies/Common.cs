namespace Persistence;

using Application.Settings;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.Configuration;

internal static partial class CommonDependencies
{
    internal static IServiceCollection AddCommonServices(
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
        Console.WriteLine( $"Environment( {environment} ), ServiceName( {settings!.ServiceName} ), KeyVaultName( {settings!.KeyVaultName} )" );
        Console.WriteLine( $"Seq-Server( {settings!.SeqServerUrl} ), Jaeger-Server( {settings!.JaegerServerUrl} )" );

        /* Important validations to make here before continue */
        if ( settings is null || !settings.DataIsValid() )
            throw new InvalidConfigurationException( $"Configuration values: {nameof( WebApiSettings )} aren't defined or invalid." );

        services.AddMongoDbServices( settings )
            .AddMemoryCache()
            .AddLoggingServices( settings )
            .AddTracingServices( settings );

        return services;
    }
}
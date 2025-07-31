namespace Persistence;

using Application.Settings;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.Configuration;

internal static partial class CommonDependencies
{
    internal static IServiceCollection AddCommonServices(
        this IServiceCollection services,
        IConfigurationBuilder configBuilder,
        IHostEnvironment hostEnvironment,
        out string dbConnUrl )
    {
        configBuilder.AddEnvironmentVariables();

        var config = configBuilder.Build();
        var settings = config.GetSection( nameof( WebApiSettings ) ).Get<WebApiSettings>();
        var environment = "Development";

        if ( hostEnvironment.IsProduction() )
        {
            environment = "Production";

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

        Console.WriteLine( $"Environment( {environment} ), ServiceName( {settings!.ServiceName} )" );
        Console.WriteLine( $"Seq-Server( {settings!.SeqServerUrl} ), KeyVaultName( {settings!.KeyVaultName} )" );

        /* Important validations to make here before continue */
        if ( settings is null || !settings.DataIsValid() )
            throw new InvalidConfigurationException( $"Configuration values: {nameof( WebApiSettings )} aren't defined or invalid." );

        services.AddDatabaseServices( settings )
            .AddLoggingServices( settings )
            .AddTracingServices( settings );

        dbConnUrl = settings.DBConnection!;
        return services;
    }
}
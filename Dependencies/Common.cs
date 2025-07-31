namespace Persistence;

using Application.Settings;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.Configuration;

internal static partial class CommonDependencies
{
    internal static IServiceCollection AddCommonServices(
        this IServiceCollection services,
        IConfigurationBuilder hostConfigBuilder,
        IHostEnvironment hostEnvironment,
        out string dbConnUrl )
    {
        hostConfigBuilder.AddEnvironmentVariables();

        var config = hostConfigBuilder.Build();
        var settings = config.GetSection( nameof( WebApiSettings ) ).Get<WebApiSettings>();

        if ( hostEnvironment.IsProduction() )
        {
            /* To work locally with Docker this value should be Empty */
            if ( !string.IsNullOrEmpty( settings!.KeyVaultName!.Trim() ) )
            {
                hostConfigBuilder.AddAzureKeyVault(
                     new Uri( $"https://{settings.KeyVaultName}.vault.azure.net/" ),
                     new DefaultAzureCredential()
                );

                config = hostConfigBuilder.Build();
                settings = config.GetSection( nameof( WebApiSettings ) ).Get<WebApiSettings>();
            }
        }

        Console.WriteLine( $"Environment( {hostEnvironment.EnvironmentName} ), ServiceName( {hostEnvironment.ApplicationName} )" );
        Console.WriteLine( $"Seq-Server( {settings!.SeqServerUrl} ), KeyVaultName( {settings!.KeyVaultName} )" );

        /* Important validations to make here before continue */
        if ( settings is null || !settings.DataIsValid() )
            throw new InvalidConfigurationException( $"Configuration values: {nameof( WebApiSettings )} aren't defined or invalid." );

        dbConnUrl = settings.DBConnection!;

        services.AddDatabaseServices( dbConnUrl )
            .AddLoggingServices( settings!.SeqServerUrl! )
            .AddTracingServices( hostEnvironment.ApplicationName );

        return services;
    }
}
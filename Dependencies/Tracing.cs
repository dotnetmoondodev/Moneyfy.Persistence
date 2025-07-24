namespace Persistence;

using Application.Settings;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

internal static partial class CommonDependencies
{
    internal static IServiceCollection AddTracingServices(
        this IServiceCollection services,
        WebApiSettings settings )
    {
        services.AddOpenTelemetry()
            .WithTracing( tracerProviderBuilder =>
            {
                tracerProviderBuilder
                    .AddSource( settings.ServiceName! )
                    .SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService( serviceName: settings.ServiceName! ) )
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddConsoleExporter();
            } );

        return services;
    }
}

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
            .ConfigureResource( resource => resource.AddService( settings.ServiceName! ) )
            .WithTracing( tracing =>
            {
                tracing.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation( opt => opt.SetDbStatementForText = true );

                tracing.AddOtlpExporter();
            } );

        return services;
    }
}

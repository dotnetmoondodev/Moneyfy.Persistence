namespace Persistence;

using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

internal static partial class CommonDependencies
{
    internal static IServiceCollection AddTracingServices(
        this IServiceCollection services,
        string serviceName )
    {
        services.AddOpenTelemetry()
            .ConfigureResource( resource => resource.AddService( serviceName ) )
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

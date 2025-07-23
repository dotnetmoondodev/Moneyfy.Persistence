namespace Persistence;

using System.Diagnostics;
using Application.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class HttpLoggingMiddleware(
    RequestDelegate next,
    ILogger<HttpLoggingMiddleware> logger )
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<HttpLoggingMiddleware> _logger = logger;

    public async Task InvokeAsync( HttpContext context )
    {
        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation( "Request( Method: {Method}, Path: {Path}, Query: {Query}, QueryString: {QueryStr} )",
            context.Request.Method, context.Request.Path, context.Request.Query, context.Request.QueryString );

        await _next( context );

        stopwatch.Stop();
        _logger.LogInformation( "Response( StatusCode: {StatusCode}, Taken: {ElapsedTime}ms )",
            context.Response.StatusCode, stopwatch.ElapsedMilliseconds );
    }
}

internal static partial class CommonDependencies
{
    internal static IServiceCollection AddLoggingServices(
        this IServiceCollection services,
        WebApiSettings settings )
    {
        services.AddLogging( loggingBuilder => loggingBuilder.AddSeq( settings.SeqServerUrl ) );
        return services;
    }
}

namespace Persistence;

using Microsoft.Extensions.Logging;

public static class WebApiLoggerCategories
{
    public const string Category = "Moneyfy";
}

public static class FastLogger
{
    /// <summary>
    /// Logger for Trace Messages
    /// </summary>
    public static readonly Action<ILogger, string, Exception?> LogTrace = LoggerMessage.Define<string>(
        logLevel: LogLevel.Trace,
        eventId: new EventId( id: 100, name: nameof( LogTrace ) ),
        formatString: "{Message}" );

    /// <summary>
    /// Logger for Debug Messages
    /// </summary>
    public static readonly Action<ILogger, string, Exception?> LogDebug = LoggerMessage.Define<string>(
        logLevel: LogLevel.Debug,
        eventId: new EventId( id: 101, name: nameof( LogDebug ) ),
        formatString: "{Message}" );

    /// <summary>
    /// Logger for Information Messages
    /// </summary>
    public static readonly Action<ILogger, string, Exception?> LogInfo = LoggerMessage.Define<string>(
        logLevel: LogLevel.Information,
        eventId: new EventId( id: 102, name: nameof( LogInfo ) ),
        formatString: "{Message}" );

    /// <summary>
    /// Logger for Warning Messages
    /// </summary>
    public static readonly Action<ILogger, string, Exception?> LogWarning = LoggerMessage.Define<string>(
        logLevel: LogLevel.Warning,
        eventId: new EventId( id: 103, name: nameof( LogWarning ) ),
        formatString: "{Message}" );

    /// <summary>
    /// Logger for Error Messages
    /// </summary>
    public static readonly Action<ILogger, string, Exception> LogError = LoggerMessage.Define<string>(
        logLevel: LogLevel.Error,
        eventId: new EventId( id: 104, name: nameof( LogError ) ),
        formatString: "{Message}" );

    /// <summary>
    /// Logger for Critical Messages
    /// </summary>
    public static readonly Action<ILogger, string, Exception?> LogCritical = LoggerMessage.Define<string>(
        logLevel: LogLevel.Critical,
        eventId: new EventId( id: 105, name: nameof( LogCritical ) ),
        formatString: "A critical error has occurred while executing the request: {Message}" );
}

using System.Diagnostics;

namespace FitnessApi.Middleware;

public class RequestLoggingMiddleware(
    RequestDelegate next,
    ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = Guid.NewGuid().ToString("N")[..8];
        context.Response.Headers["X-Correlation-Id"] = correlationId;

        var method = context.Request.Method;
        var path = context.Request.Path;

        logger.LogInformation(
            "Request {Method} {Path} [CorrelationId: {CorrelationId}]",
            method, path, correlationId);

        var stopwatch = Stopwatch.StartNew();

        await next(context);

        stopwatch.Stop();
        var statusCode = context.Response.StatusCode;

        logger.LogInformation(
            "Response {StatusCode} {Method} {Path} [CorrelationId: {CorrelationId}] took {ElapsedMs}ms",
            statusCode, method, path, correlationId, stopwatch.ElapsedMilliseconds);
    }
}
using System.Diagnostics;
using ActionIn.Core.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ActionIn.Core.Infrastructure.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        long startedAt = Stopwatch.GetTimestamp();

        TResponse response = await next(cancellationToken);

        double elapsedMs = Stopwatch.GetElapsedTime(startedAt).TotalMilliseconds;

        if (response is Result result && result.IsFailure)
        {
            LogLevel level = result.Error.Type == ErrorType.Failure
                ? LogLevel.Error
                : LogLevel.Warning;

            logger.Log(
                level,
                "{RequestName} failed with {ErrorCode} ({ErrorType}) in {ElapsedMs:0} ms",
                requestName, result.Error.Code, result.Error.Type, elapsedMs);
        }
        else
        {
            logger.LogInformation("{RequestName} completed in {ElapsedMs:0} ms",
                requestName, elapsedMs);
        }

        return response;
    }
}

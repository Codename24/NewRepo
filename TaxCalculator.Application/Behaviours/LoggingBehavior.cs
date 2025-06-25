using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace TaxCalculator.Application.Behaviours
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling Request: {RequestName}, Data: {@Request}", typeof(TRequest).Name, request);

            var stopwatch = Stopwatch.StartNew();
            var response = await next();
            stopwatch.Stop();

            _logger.LogInformation("Completed Request: {RequestName} in {ElapsedMilliseconds}ms, Response: {@Response}",
                typeof(TRequest).Name, stopwatch.ElapsedMilliseconds, response);

            return response;
        }
    }
}

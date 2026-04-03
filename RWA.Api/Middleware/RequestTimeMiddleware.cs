using System.Diagnostics;

namespace RWA.Api.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly Stopwatch _stopWatch;
        private readonly ILogger<RequestTimeMiddleware> _logger;


        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _stopWatch = new Stopwatch();
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopWatch.Start();
            await next.Invoke(context);
            _stopWatch.Stop();

            var elapsedMilliseconds = _stopWatch.ElapsedMilliseconds;

            if(elapsedMilliseconds/1000 > 4)
            {
                var message = $"Request [{context.Request.Method}] at {context.Request.Path} took {elapsedMilliseconds} ms";

                _logger.LogInformation(message);
            }
        }
    }
}

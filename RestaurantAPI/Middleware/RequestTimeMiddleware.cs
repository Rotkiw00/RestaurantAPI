
using System.Diagnostics;

namespace RestaurantAPI.Middleware
{
	public class RequestTimeMiddleware : IMiddleware
	{
		private readonly ILogger<RequestTimeMiddleware> _logger;
		private readonly Stopwatch _stopwatch;

		public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
		{
			_logger = logger;
			_stopwatch = new Stopwatch();
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			_stopwatch.Start();
			await next.Invoke(context);
			_stopwatch.Stop();

			long elapsedRequestTime = _stopwatch.ElapsedMilliseconds;

			if (elapsedRequestTime / 1000 > 4)
			{
				string message = $"Request {context.Request.Method} on path {context.Request.Path} took {elapsedRequestTime} ms.";
				_logger.LogInformation(message);
			}
		}
	}
}

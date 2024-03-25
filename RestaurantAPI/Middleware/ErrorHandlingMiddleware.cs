﻿using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Middleware
{
	public class ErrorHandlingMiddleware : IMiddleware
	{
		private readonly ILogger<ErrorHandlingMiddleware> _logger;

		public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
			_logger = logger;
		}

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next.Invoke(context);
			}
			catch (NotFoundException notFoundEx)
			{
				context.Response.StatusCode = 404;
				await context.Response.WriteAsync(notFoundEx.Message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);

				context.Response.StatusCode = 500;
				await context.Response.WriteAsync("Something went wrong...");
			}
		}
	}
}

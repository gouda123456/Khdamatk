
using System;

namespace Khdamatk.Server.MiddleWares;

public class GlobalErrorHandling(ILogger<GlobalErrorHandling> logger) : IMiddleware
{
	private readonly ILogger<GlobalErrorHandling> logger = logger;

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		try
		{
			await next(context);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Something went wrong: {Message}", ex.Message);

			context.Response.StatusCode = 500;
			context.Response.ContentType = "application/json";
			var errorResponse = Failure(500, "Internal Server Error", "An unexpected error occurred while processing the request.");
			await context.Response.WriteAsJsonAsync(errorResponse);
		} 
	}
}

using System;
using System.Threading.Tasks;
using GetWell.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GetWell.Dashboard;

public class CustomMiddleware : ICustomMiddleware
{
	private readonly ILogger<CustomMiddleware> _logger;

	public CustomMiddleware(ILogger<CustomMiddleware> logger)
	{
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context, RequestDelegate next)
	{
		var ip = context.Connection.RemoteIpAddress;
		var lang = context.Request.Headers.AcceptLanguage;
		var role = context.User.IsInRole(UserRoles.Doctor);
		_logger.Log(LogLevel.Information, $"user ip: {ip}");
		Console.WriteLine($"user ip: {ip}");

		await next(context);
	}
}
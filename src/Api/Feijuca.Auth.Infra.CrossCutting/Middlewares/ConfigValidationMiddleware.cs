using Feijuca.Auth.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Feijuca.Auth.Infra.CrossCutting.Middlewares
{
    public class ConfigValidationMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context, IConfigRepository configRepository)
        {
            if (context.Request.Path.StartsWithSegments("/api/v1/config", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            var configResult = configRepository.GetConfigAsync();

            if (configResult == null)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;

                var errorResponse = new
                {
                    StatusCode = StatusCodes.Status503ServiceUnavailable,
                    Message = "Configs not found! Please, use the endpoint to insert your Keycloak configs before start.",
                    Documentation = "https://coderaw-io.github.io/Feijuca.Auth/docs/gettingStarted.html"
                };

                context.Response.ContentType = "application/json";
                var jsonResponse = JsonSerializer.Serialize(errorResponse);

                await context.Response.WriteAsync(jsonResponse);
                return;
            }

            await _next(context);
        }
    }
}

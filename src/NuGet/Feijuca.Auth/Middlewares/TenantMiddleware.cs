﻿using Feijuca.Auth.Models;
using Feijuca.Auth.Services;
using Microsoft.AspNetCore.Http;

namespace Feijuca.Auth.Middlewares
{
    public class TenantMiddleware(RequestDelegate next, TenantMiddlewareOptions options)
    {
        private static readonly List<string> _defaultUrls = ["scalar", "openapi", "events", "favicon.ico", "swagger"];
        private readonly List<string> _availableUrls = [.. _defaultUrls.Union(options.AvailableUrls ?? Enumerable.Empty<string>(), StringComparer.OrdinalIgnoreCase)];

        public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
        {
            var token = tenantService.GetToken();
            var path = context.Request.Path.Value!;
            if (_availableUrls.Exists(path.Contains) && string.IsNullOrEmpty(token))
            {
                await next(context);
                return;
            }

            var tenants = tenantService.GetTenants();
            var user = tenantService.GetUser();

            if (!tenants.Any() || user.Id == Guid.Empty)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var response = new { error = "Jwt token authorization header is required." };

                await context.Response.WriteAsJsonAsync(response);

                return;
            }

            tenantService.SetTenants(tenants);
            tenantService.SetUser(user);

            await next(context);
        }
    }
}

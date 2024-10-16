﻿using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Feijuca.Auth.Infra.CrossCutting.Handlers
{
    public sealed class GlobalExceptionHandler() : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var code = exception switch
            {
                ValidationException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                HttpRequestException => HttpStatusCode.InternalServerError,
                _ => HttpStatusCode.InternalServerError,
            };

            var errors = new List<string>
            {
                exception.Message + " - " + exception.StackTrace,
            };

            foreach (var key in exception.Data.Keys)
            {
                errors.Add(exception.Data[key]?.ToString() ?? "");
            }

            var problemDetails = new ProblemDetails
            {
                Status = (int)code,
                Title = string.Join(",", errors)
            };

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}

﻿using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Infra.CrossCutting.Filters;
using Flurl;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Feijuca.Auth.Infra.CrossCutting.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, KeycloakSettings? keycloakSettings)
        {
            var realmName = keycloakSettings?.Realms?.FirstOrDefault(x => x.DefaultSwaggerTokenGeneration)?.Name ?? "";            

            if (keycloakSettings is not null && !string.IsNullOrEmpty(realmName))
            {
                var url = keycloakSettings?.ServerSettings.Url
                    .AppendPathSegment("realms")
                    .AppendPathSegment(realmName)
                    .AppendPathSegment($"/protocol/openid-connect/token");

                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Feijuca.Auth.Api", Version = "v1" });

                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            Password = new OpenApiOAuthFlow
                            {
                                TokenUrl = new Uri(url),
                            }
                        }
                    });

                    c.OperationFilter<TenantHeaderFilter>();
                    c.OperationFilter<AuthorizeCheckOperationFilter>();
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Feijuca.Auth.Api.xml"));
                });
            }

            services.AddSwaggerGen();
            return services;
        }
    }
}

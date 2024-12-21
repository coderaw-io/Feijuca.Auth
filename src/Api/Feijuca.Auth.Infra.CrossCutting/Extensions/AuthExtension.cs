using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Entities;
using Feijuca.Auth.Domain.Interfaces;
using Feijuca.Auth.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace Feijuca.Auth.Infra.CrossCutting.Extensions
{
    public static class AuthExtension
    {
        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, out KeycloakSettings serverSettings)
        {
            var serviceProvider = services.BuildServiceProvider();
            var KeycloakSettingsRepository = serviceProvider.GetRequiredService<IConfigRepository>();
            serverSettings = KeycloakSettingsRepository.GetConfigAsync().GetAwaiter().GetResult();

            if (serverSettings is not null)
            {
                services.AddHttpContextAccessor();
                services.AddSingleton<JwtSecurityTokenHandler>();
                services.AddKeyCloakAuth(serverSettings.Client, serverSettings.ServerSettings, serverSettings.Realms ?? []);

                services.AddSingleton(new TokenCredentials()
                {
                    Client_Secret = serverSettings.Secrets.ClientSecret,
                    Client_Id = serverSettings.Client.ClientId,
                    ServerUrl = serverSettings.ServerSettings.Url
                });

                return services;
            }

            services.AddSingleton(new TokenCredentials());
            return services;
        }
    }
}

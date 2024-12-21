using Feijuca.Auth.Common.Models;
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

                return services;
            }

            return services;
        }
    }
}

using Feijuca.Auth.Domain.Interfaces;
using Feijuca.Auth.Extensions;
using Feijuca.Auth.Models;

using Microsoft.Extensions.DependencyInjection;

using System.IdentityModel.Tokens.Jwt;

namespace Feijuca.Auth.Infra.CrossCutting.Extensions
{
    public static class AuthExtension
    {
        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, out AuthSettings authSettings)
        {
            var serviceProvider = services.BuildServiceProvider();
            var authSettingsRepository = serviceProvider.GetRequiredService<IConfigRepository>();
            authSettings = authSettingsRepository.GetConfig();

            services.AddHttpContextAccessor();
            services.AddSingleton<JwtSecurityTokenHandler>();
            services.AddKeyCloakAuth(authSettings!);

            return services;
        }
    }
}

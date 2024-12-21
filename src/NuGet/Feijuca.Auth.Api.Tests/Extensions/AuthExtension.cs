using Feijuca.Auth.Extensions;
using Feijuca.Auth.Models;
using System.IdentityModel.Tokens.Jwt;

namespace Feijuca.Auth.Api.Tests.Extensions;

public static class AuthExtension
{
    public static IServiceCollection AddApiAuthentication(this IServiceCollection services, ServerSettings authSettings)
    {
        var xx = authSettings;
        var client = new Client { MasterClientId = xx.Url };

        services.AddHttpContextAccessor();
        services.AddSingleton<JwtSecurityTokenHandler>();
        services.AddKeyCloakAuth(client, null!, null!, null!);

        return services;
    }
}


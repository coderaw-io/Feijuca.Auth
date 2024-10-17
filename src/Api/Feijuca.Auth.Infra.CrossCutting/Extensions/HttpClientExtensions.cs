using Feijuca.Auth.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Feijuca.Auth.Infra.CrossCutting.Extensions
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services, string authServerUrl)
        {
            services.AddHttpClient("KeycloakClient", client =>
            {
                client.BaseAddress = new Uri(authServerUrl);
            });

            return services;
        }
    }
}

using Feijuca.Auth.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Feijuca.Auth.Infra.CrossCutting.Extensions
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient("KeycloakClient", client =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var KeycloakSettingsRepository = serviceProvider.GetRequiredService<IConfigRepository>();
                var serverSettings = KeycloakSettingsRepository.GetConfigAsync().GetAwaiter().GetResult();

                if (serverSettings is not null)
                {
                    client.BaseAddress = new Uri(serverSettings.ServerSettings.Url);
                }
            });

            return services;
        }
    }
}

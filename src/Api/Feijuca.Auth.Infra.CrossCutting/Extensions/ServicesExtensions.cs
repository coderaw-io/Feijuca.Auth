using Feijuca.Auth.Domain.Interfaces;
using Feijuca.Auth.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Feijuca.Auth.Infra.CrossCutting.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IRealmService, RealmService>();

            return services;
        }

    }
}

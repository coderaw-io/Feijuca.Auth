﻿using Application.Commands.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.CrossCutting.Extensions
{
    public static class MediatRExtension
    {
        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(
                    x => x.RegisterServicesFromAssemblies(typeof(CreateUserCommand).Assembly));

            return services;
        }
    }
}
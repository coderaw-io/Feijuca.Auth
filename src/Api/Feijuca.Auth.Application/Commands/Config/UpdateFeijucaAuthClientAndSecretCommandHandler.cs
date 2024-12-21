using Feijuca.Auth.Common;
using Feijuca.Auth.Common.Errors;
using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Interfaces;
using MediatR;

namespace Feijuca.Auth.Application.Commands.Config
{
    public class UpdateFeijucaAuthClientAndSecretCommandHandler(IClientRepository clientRepository, IConfigRepository configRepository) : IRequestHandler<UpdateFeijucaConfigWithClientIdAndSecretCommandHandler, Result<bool>>
    {
        public async Task<Result<bool>> Handle(UpdateFeijucaConfigWithClientIdAndSecretCommandHandler request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetClientAsync(Constants.FeijucaApiClientName, cancellationToken);
            var config = await configRepository.GetConfigAsync();

            var configRealm = config?.Realms?.First();
            if (configRealm != null)
            {
                configRealm.FeijucaAuthClient = new Models.FeijucaAuthClient
                {
                    FeijucaAuthClientId = client.Response.ClientId,
                    FeijucaAuthClientSecret = client.Response.Secret,
                };

                config!.Realms = config.Realms!.Select(r =>
                {
                    if (r.Name == configRealm.Name)
                    {
                        r.FeijucaAuthClient = configRealm.FeijucaAuthClient;
                    }

                    return r;
                }).ToList();

                await configRepository.UpdateRealmConfigAsync(config.Id, config);

                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(RealmErrors.UpdateRealmError);
        }
    }
}

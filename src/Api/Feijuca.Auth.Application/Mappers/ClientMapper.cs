using Feijuca.Auth.Application.Requests.Client;
using Feijuca.Auth.Domain.Entities;

namespace Feijuca.Auth.Application.Mappers
{
    public static class ClientMapper
    {
        public static ClientEntity ToClientEntity(this AddClientRequest addClientRequest)
        {
            return new ClientEntity()
            {
                ClientId = addClientRequest.ClientId,
                Description = addClientRequest.Description,
                RedirectUrls = addClientRequest.Urls
            };
        }
    }
}

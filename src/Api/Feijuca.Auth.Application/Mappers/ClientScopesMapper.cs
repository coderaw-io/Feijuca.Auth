using Feijuca.Auth.Application.Requests.ClientScopes;
using Feijuca.Auth.Domain.Entities;

namespace Feijuca.Auth.Application.Mappers
{
    public static class ClientScopesMapper
    {
        public static ClientScopesEntity ToClientScopesEntity(this AddClientScopesRequest addClientScopesRequest)
        {
            return new ClientScopesEntity(addClientScopesRequest.Name, addClientScopesRequest.Description, addClientScopesRequest.IncludeInTokenScope);
        }
    }
}

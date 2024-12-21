using Feijuca.Auth.Application.Requests.ClientScopes;
using Feijuca.Auth.Application.Responses;
using Feijuca.Auth.Domain.Entities;

namespace Feijuca.Auth.Application.Mappers
{
    public static class ClientScopesMapper
    {
        public static ClientScopesEntity ToClientScopesEntity(this AddClientScopesRequest addClientScopesRequest)
        {
            return new ClientScopesEntity(addClientScopesRequest.Name, addClientScopesRequest.Description, addClientScopesRequest.IncludeInTokenScope);
        }

        public static IEnumerable<ClientScopesResponse> ToClientScopesResponse(this IEnumerable<ClientScopeEntity> clientScopeEntities)
        {
            var list = new List<ClientScopesResponse>();

            foreach (var item in clientScopeEntities)
            {
                list.Add(new ClientScopesResponse(item.Id, item.Name, item.Description, item.Protocol, item.Attributes, item.ProtocolMappers.ToProtocolMapperResponse()));
            }

            return list;
        }

        public static IEnumerable<ProtocolMapperResponse> ToProtocolMapperResponse(this IEnumerable<ProtocolMapperEntity> protocolMapperEntity)
        {
            var list = new List<ProtocolMapperResponse>();

            if (protocolMapperEntity != null)
            {
                foreach (var item in protocolMapperEntity)
                {
                    list.Add(new ProtocolMapperResponse(item.Id, item.Name, item.Protocol, item.ProtocolMapperType, item.ConsentRequired, item.Config));
                }
            }

            return list;
        }
    }
}

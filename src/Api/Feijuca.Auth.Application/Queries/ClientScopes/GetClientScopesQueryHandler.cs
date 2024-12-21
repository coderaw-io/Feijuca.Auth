using Feijuca.Auth.Application.Mappers;
using Feijuca.Auth.Application.Responses;
using Feijuca.Auth.Domain.Interfaces;
using MediatR;

namespace Feijuca.Auth.Application.Queries.ClientScopes
{
    public class GetClientScopesQueryHandler(IClientScopesRepository clientScopesRepository) : IRequestHandler<GetClientScopesQuery, IEnumerable<ClientScopesResponse>>
    {
        public async Task<IEnumerable<ClientScopesResponse>> Handle(GetClientScopesQuery request, CancellationToken cancellationToken)
        {
            var scopes = await clientScopesRepository.GetClientScopesAsync(cancellationToken);
            return scopes.ToClientScopesResponse();
        }
    }
}

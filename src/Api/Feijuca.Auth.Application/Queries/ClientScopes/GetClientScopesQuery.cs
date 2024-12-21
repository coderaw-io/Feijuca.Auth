using Feijuca.Auth.Application.Responses;
using MediatR;

namespace Feijuca.Auth.Application.Queries.ClientScopes
{
    public record GetClientScopesQuery() : IRequest<IEnumerable<ClientScopesResponse>>;
}

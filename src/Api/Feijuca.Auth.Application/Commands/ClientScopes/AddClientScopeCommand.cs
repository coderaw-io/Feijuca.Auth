using Feijuca.Auth.Application.Requests.ClientScopes;
using MediatR;

namespace Feijuca.Auth.Application.Commands.ClientScopes
{
    public record AddClientScopeCommand(AddClientScopesRequest AddClientScopesRequest) : IRequest<bool>;
}

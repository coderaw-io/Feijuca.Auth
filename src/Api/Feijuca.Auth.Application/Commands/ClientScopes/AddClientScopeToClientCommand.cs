using MediatR;

namespace Feijuca.Auth.Application.Commands.ClientScopes
{
    public record AddClientScopeToClientCommand(string ClientId, string ClientScopeId) : IRequest<bool>;
}

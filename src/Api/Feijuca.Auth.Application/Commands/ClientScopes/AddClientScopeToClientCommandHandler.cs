using Feijuca.Auth.Domain.Interfaces;
using MediatR;

namespace Feijuca.Auth.Application.Commands.ClientScopes
{
    public class AddClientScopeToClientCommandHandler(IClientScopesRepository clientScopesRepository) : IRequestHandler<AddClientScopeToClientCommand, bool>
    {
        public async Task<bool> Handle(AddClientScopeToClientCommand request, CancellationToken cancellationToken)
        {
            var result = await clientScopesRepository.AddClientScopeToClientAsync(request.ClientId, request.ClientScopeId, cancellationToken);
            return result;
        }
    }
}

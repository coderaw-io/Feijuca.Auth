using MediatR;

namespace Feijuca.Auth.Application.Commands.ClientScopes
{
    public class AddClientScopeCommandHandler : IRequestHandler<AddClientScopeCommand, bool>
    {
        public Task<bool> Handle(AddClientScopeCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

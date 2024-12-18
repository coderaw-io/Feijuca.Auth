using Feijuca.Auth.Application.Mappers;
using Feijuca.Auth.Domain.Interfaces;
using MediatR;

namespace Feijuca.Auth.Application.Commands.Client
{
    public class AddClientCommandHandler(IClientRepository clientRepository) : IRequestHandler<AddClientCommand, bool>
    {
        public async Task<bool> Handle(AddClientCommand request, CancellationToken cancellationToken)
        {
            var client = request.AddClientRequest.ToClientEntity();
            var result = await clientRepository.CreateClientAsync(client, cancellationToken);
            return result;
        }
    }
}

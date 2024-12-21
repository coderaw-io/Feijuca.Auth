using Feijuca.Auth.Application.Mappers;
using Feijuca.Auth.Common.Errors;
using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Interfaces;
using MediatR;

namespace Feijuca.Auth.Application.Commands.Client
{
    public class AddClientCommandHandler(IClientRepository clientRepository) : IRequestHandler<AddClientCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(AddClientCommand request, CancellationToken cancellationToken)
        {
            var client = request.AddClientRequest.ToClientEntity();
            var result = await clientRepository.CreateClientAsync(client, cancellationToken);

            if (result)
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(ClientErrors.CreateClientError);
        }
    }
}

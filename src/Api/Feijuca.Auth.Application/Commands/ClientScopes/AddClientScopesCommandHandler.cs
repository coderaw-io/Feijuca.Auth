using Feijuca.Auth.Application.Mappers;
using Feijuca.Auth.Common.Errors;
using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Interfaces;
using MediatR;

namespace Feijuca.Auth.Application.Commands.ClientScopes
{
    public class AddClientScopesCommandHandler(IClientScopesRepository clientScopesRepository) : IRequestHandler<AddClientScopesCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(AddClientScopesCommand command, CancellationToken cancellationToken)
        {
            foreach (var clientScope in command.AddClientScopesRequest)
            {
                var scopeEntity = clientScope.ToClientScopesEntity();
                var result = await clientScopesRepository.AddClientScopesAsync(scopeEntity, cancellationToken);

                if (!result)
                {
                    return Result<bool>.Failure(ClientScopesErrors.CreateClientScopesError);
                }
            }


            return Result<bool>.Success(true);
        }
    }
}

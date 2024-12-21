using Feijuca.Auth.Common.Errors;
using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Interfaces;
using MediatR;

namespace Feijuca.Auth.Application.Commands.ClientRole
{
    public class AddClientRoleCommandHandler(IClientRoleRepository roleRepository) : IRequestHandler<AddClientRoleCommand, Result<bool>>
    {
        private readonly IClientRoleRepository _roleRepository = roleRepository;

        public async Task<Result<bool>> Handle(AddClientRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _roleRepository.AddRoleAsync(request.AddRoleRequest.Id, request.AddRoleRequest.Name, request.AddRoleRequest.Description, cancellationToken);
            if (result.IsSuccess)
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(RoleErrors.AddRoleErrors);
        }
    }
}

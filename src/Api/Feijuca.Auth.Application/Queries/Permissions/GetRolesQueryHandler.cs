using Feijuca.Auth.Common.Errors;
using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Application.Responses;
using Feijuca.Auth.Domain.Interfaces;
using MediatR;

namespace Feijuca.Auth.Application.Queries.Permissions
{
    public class GetRolesQueryHandler(IClientRepository clientRepository, IClientRoleRepository roleRepository) : IRequestHandler<GetRolesQuery, Result<IEnumerable<ClientRoleResponse>>>
    {
        private readonly IClientRepository _clientRepository = clientRepository;
        private readonly IClientRoleRepository _roleRepository = roleRepository;

        public async Task<Result<IEnumerable<ClientRoleResponse>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var result = await _clientRepository.GetClientsAsync(cancellationToken);
            if (result.IsSuccess)
            {
                var roleResponse = new List<ClientRoleResponse>();
                foreach (var client in result.Response)
                {
                    var rolesResult = await _roleRepository.GetRolesForClientAsync(client.Id, cancellationToken);

                    if (rolesResult.IsSuccess)
                    {
                        var rolesResponse = rolesResult.Response.Select(x => new RoleResponse(x.Id, x.Name, x.Description ?? "", false, false, string.Empty));
                        roleResponse.Add(new ClientRoleResponse(client.ClientId, client.Id, rolesResponse));
                    }
                }

                return Result<IEnumerable<ClientRoleResponse>>.Success(roleResponse);
            }

            return Result<IEnumerable<ClientRoleResponse>>.Failure(RoleErrors.GetRoleErrors);
        }
    }
}

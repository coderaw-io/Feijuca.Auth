using Feijuca.Auth.Common.Errors;
using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Interfaces;
using MediatR;

namespace Feijuca.Auth.Application.Commands.GroupRoles;

public class RemoveRoleFromGroupCommandHandler(IGroupRepository groupRepository, IGroupRolesRepository roleGroupRepository, IClientRoleRepository roleRepository) : IRequestHandler<RemoveRoleFromGroupCommand, Result<bool>>
{
    private readonly IGroupRepository _groupRepository = groupRepository;
    private readonly IGroupRolesRepository _roleGroupRepository = roleGroupRepository;
    private readonly IClientRoleRepository _roleRepository = roleRepository;

    public async Task<Result<bool>> Handle(RemoveRoleFromGroupCommand command, CancellationToken cancellationToken)
    {
        var groupsResult = await _groupRepository.GetAllAsync(cancellationToken);
        if (groupsResult.IsSuccess && groupsResult.Response.Any(x => x.Id == command.GroupId))
        {
            var rolesResult = await _roleRepository.GetRolesForClientAsync(command.RemoveRoleFromGroupRequest.Id, cancellationToken);
            var existingRule = rolesResult.Response.FirstOrDefault(x => x.Id == command.RemoveRoleFromGroupRequest.RoleId);
            if (rolesResult.IsSuccess && existingRule != null)
            {
                await _roleGroupRepository.RemoveRoleFromGroupAsync(
                    command.RemoveRoleFromGroupRequest.Id,
                    command.GroupId,
                    existingRule.Id,
                    existingRule.Name,
                    cancellationToken);

                return Result<bool>.Success(true);
            }
        }

        return Result<bool>.Failure(GroupRolesErrors.RemovingRoleFromGroupError);
    }
}

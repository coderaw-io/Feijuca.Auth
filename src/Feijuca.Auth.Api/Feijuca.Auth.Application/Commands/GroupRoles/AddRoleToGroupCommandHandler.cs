﻿using Feijuca.Auth.Common.Errors;
using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Interfaces;

using MediatR;

namespace Feijuca.Auth.Application.Commands.GroupRoles;

public class AddRoleToGroupCommandHandler(IGroupRepository groupRepository, IGroupRolesRepository roleGroupRepository, IRoleRepository roleRepository) : IRequestHandler<AddRoleToGroupCommand, Result<bool>>
{
    private readonly IGroupRepository _groupRepository = groupRepository;
    private readonly IGroupRolesRepository _roleGroupRepository = roleGroupRepository;
    private readonly IRoleRepository _roleRepository = roleRepository;

    public async Task<Result<bool>> Handle(AddRoleToGroupCommand request, CancellationToken cancellationToken)
    {
        var groupsResult = await _groupRepository.GetAllAsync(request.Tenant);
        var rolesResult = await _roleRepository.GetRolesForClientAsync(request.Tenant, request.AddRoleToGroupRequest.ClientId);

        if (groupsResult.IsSuccess && rolesResult.IsSuccess)
        {
            var group = groupsResult.Response.FirstOrDefault(x => x.Id == request.GroupId);
            var role = rolesResult.Response.FirstOrDefault(x => x.Id == request.AddRoleToGroupRequest.RoleId);

            if (group != null && role != null)
            {

                var result = await _roleGroupRepository.AddRoleToGroupAsync(request.Tenant,
                    group.Id,
                    request.AddRoleToGroupRequest.ClientId,
                    role.Id,
                    role.Name);

                if (result.IsSuccess)
                {
                    return Result<bool>.Success(true);
                }
            }
        }

        return Result<bool>.Failure(GroupRolesErrors.ErrorAddRoleToGroup);
    }
}

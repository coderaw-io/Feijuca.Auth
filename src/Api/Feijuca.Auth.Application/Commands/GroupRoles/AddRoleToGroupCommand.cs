using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Application.Requests.GroupRoles;
using MediatR;

namespace Feijuca.Auth.Application.Commands.GroupRoles
{
    public record AddRoleToGroupCommand(string GroupId, RoleToGroupRequest AddRoleToGroupRequest) : IRequest<Result<bool>>;
}

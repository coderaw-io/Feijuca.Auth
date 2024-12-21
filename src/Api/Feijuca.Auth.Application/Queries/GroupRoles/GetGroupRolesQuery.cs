using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Application.Responses;
using MediatR;

namespace Feijuca.Auth.Application.Queries.GroupRoles
{
    public record GetGroupRolesQuery(string GroupId) : IRequest<Result<IEnumerable<GroupRolesResponse>>>;
}

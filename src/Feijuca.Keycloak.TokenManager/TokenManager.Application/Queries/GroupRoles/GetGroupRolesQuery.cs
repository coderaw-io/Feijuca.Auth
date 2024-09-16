﻿using MediatR;

using TokenManager.Application.Responses;
using TokenManager.Common.Models;

namespace TokenManager.Application.Queries.GroupRoles
{
    public record GetGroupRolesQuery(string Tenant, string GroupId) : IRequest<Result<IEnumerable<GroupRolesResponse>>>;
}
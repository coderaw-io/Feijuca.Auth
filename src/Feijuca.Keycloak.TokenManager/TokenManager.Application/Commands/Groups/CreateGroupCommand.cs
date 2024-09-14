﻿using MediatR;

using TokenManager.Application.Requests.User;
using TokenManager.Common.Models;

namespace TokenManager.Application.Commands.Groups
{
    public record CreateGroupCommand(string Tenant, AddGroupRequest AddGroupRequest) : IRequest<Result>;
}
﻿using Feijuca.Auth.Common.Models;

using MediatR;

namespace Feijuca.Auth.Application.Commands.GroupUser
{
    public record AddUserToGroupCommand(Guid UserId, Guid GroupId) : IRequest<Result<bool>>;
}

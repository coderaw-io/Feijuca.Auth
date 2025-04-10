﻿using Feijuca.Auth.Application.Requests.UsersAttributes;
using Feijuca.Auth.Common.Models;
using MediatR;

namespace Feijuca.Auth.Application.Commands.UserAttributes
{
    public record UpdateUserAttributesCommand(string Username, UserAttributeRequest UpdateUserAttributeRequest) : IRequest<Result<bool>>;
}

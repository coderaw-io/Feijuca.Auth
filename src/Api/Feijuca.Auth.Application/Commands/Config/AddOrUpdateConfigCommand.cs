﻿using Feijuca.Auth.Common.Models;
using MediatR;

namespace Feijuca.Auth.Application.Commands.Config
{
    public record AddOrUpdateConfigCommand(KeycloakSettings Request) : IRequest<Result<bool>>;
}

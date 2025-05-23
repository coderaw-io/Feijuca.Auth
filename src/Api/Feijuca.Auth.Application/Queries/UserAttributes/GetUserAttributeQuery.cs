﻿using Feijuca.Auth.Common.Models;
using MediatR;

namespace Feijuca.Auth.Application.Queries.UserAttributes
{
    public record GetUserAttributeQuery(string Username) : IRequest<Result<Dictionary<string, string[]>>>;
}

using Feijuca.Auth.Common.Models;

using MediatR;

namespace Feijuca.Auth.Application.Commands.Group
{
    public record DeleteGroupCommand(string Id) : IRequest<Result<bool>>;
}

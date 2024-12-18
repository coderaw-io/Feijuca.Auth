using Feijuca.Auth.Application.Requests.Client;
using MediatR;

namespace Feijuca.Auth.Application.Commands.Client
{
    public record AddClientCommand(AddClientRequest AddClientRequest) : IRequest<bool>;
}

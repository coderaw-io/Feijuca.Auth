using Feijuca.Auth.Application.Requests.Realm;
using MediatR;

namespace Feijuca.Auth.Application.Commands.Realm
{
    public record AddRealmCommand(AddRealmRequest AddRealmRequest) : IRequest<bool>;
}

using Feijuca.Auth.Application.Requests.Realm;
using Feijuca.Auth.Common.Models;

using MediatR;

namespace Feijuca.Auth.Application.Commands.Realm
{
    public record AddRealmsCommand(IEnumerable<AddRealmRequest> AddRealmsRequest) : IRequest<Result<bool>>;
}

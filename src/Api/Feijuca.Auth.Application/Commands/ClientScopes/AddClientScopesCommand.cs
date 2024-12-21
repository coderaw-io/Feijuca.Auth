using Feijuca.Auth.Application.Requests.ClientScopes;
using Feijuca.Auth.Common.Models;
using MediatR;

namespace Feijuca.Auth.Application.Commands.ClientScopes
{
    public record AddClientScopesCommand(IEnumerable<AddClientScopesRequest> AddClientScopesRequest) : IRequest<Result<bool>>;
}

using Feijuca.Auth.Application.Responses;

using MediatR;

namespace Feijuca.Auth.Application.Queries.Clients
{
    public record GetAllClientsQuery() : IRequest<IEnumerable<ClientResponse>>;
}

using Feijuca.Auth.Application.Responses;
using Feijuca.Auth.Domain.Interfaces;
using MediatR;

namespace Feijuca.Auth.Application.Queries.Clients
{
    public class GetAllClientsQueryHandler(IClientRepository clientRepository) : IRequestHandler<GetAllClientsQuery, IEnumerable<ClientResponse>>
    {
        private readonly IClientRepository _clientRepository = clientRepository;

        public async Task<IEnumerable<ClientResponse>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            var clientsResult = await _clientRepository.GetClientsAsync(cancellationToken);

            if (clientsResult.IsSuccess)
            {
                var clients = clientsResult.Response.Select(x => new ClientResponse(x.Enabled, x.Id, x.ClientId));
                return clients;
            }

            return [];
        }
    }
}

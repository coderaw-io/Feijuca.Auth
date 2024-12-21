using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Entities;

namespace Feijuca.Auth.Domain.Interfaces
{
    public interface IClientRepository : IBaseRepository
    {
        Task<Result<IEnumerable<ClientEntity>>> GetClientsAsync(CancellationToken cancellationToken);
        Task<Result<ClientEntity>> GetClientAsync(string clientId, CancellationToken cancellationToken);
        Task<bool> CreateClientAsync(ClientEntity client, CancellationToken cancellationToken);
    }
}

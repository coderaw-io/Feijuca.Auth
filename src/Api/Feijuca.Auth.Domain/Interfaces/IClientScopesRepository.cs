using Feijuca.Auth.Domain.Entities;

namespace Feijuca.Auth.Domain.Interfaces
{
    public interface IClientScopesRepository : IBaseRepository
    {
        Task<bool> AddClientScopesAsync(ClientScopesEntity clientScopesEntity, CancellationToken cancellationToken);
    }
}

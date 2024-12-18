using Feijuca.Auth.Domain.Entities;

namespace Feijuca.Auth.Domain.Interfaces
{
    public interface IRealmRepository : IBaseRepository
    {
        Task<string> GetRealmConfigAsync(string name, CancellationToken cancellationToken);
        Task<bool> CreateRealmAsync(RealmEntity realm, CancellationToken cancellationToken);
    }
}

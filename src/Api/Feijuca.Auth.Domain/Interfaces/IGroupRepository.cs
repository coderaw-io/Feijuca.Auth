using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Entities;
using Feijuca.Auth.Domain.Filters;

namespace Feijuca.Auth.Domain.Interfaces
{
    public interface IGroupRepository : IBaseRepository
    {
        Task<Result<IEnumerable<Group>>> GetAllAsync(CancellationToken cancellationToken);
        Task<Result> CreateAsync(string name, Dictionary<string, string[]> attributes, CancellationToken cancellationToken);
        Task<Result> DeleteAsync(string id, CancellationToken cancellationToken);
        Task<Result<IEnumerable<User>>> GetUsersInGroupAsync(string id, UserFilters userFilters, CancellationToken cancellationToken);
    }
}

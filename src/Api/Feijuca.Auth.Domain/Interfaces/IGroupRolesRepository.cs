using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Entities;

namespace Feijuca.Auth.Domain.Interfaces
{
    public interface IGroupRolesRepository : IBaseRepository
    {
        Task<Result<bool>> AddRoleToGroupAsync(string groupId, string clientId, Guid roleId, string roleName, CancellationToken cancellationToken);
        Task<Result<IEnumerable<ClientMapping>>> GetGroupRolesAsync(string groupId, CancellationToken cancellationToken);
        Task<Result> RemoveRoleFromGroupAsync(string clientId, string groupId, Guid roleId, string roleName, CancellationToken cancellationToken);
    }
}

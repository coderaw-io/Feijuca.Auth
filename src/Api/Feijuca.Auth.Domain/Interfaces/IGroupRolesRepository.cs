using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Entities;

namespace Feijuca.Auth.Domain.Interfaces
{
    public interface IGroupRolesRepository
    {
        Task<Result<bool>> AddRoleToGroupAsync(string tenant, Guid groupId, Guid clientId, Guid roleId, string roleName);
        Task<Result<IEnumerable<ClientMapping>>> GetGroupRolesAsync(string tenant, Guid groupId);
        Task<Result> RemoveRoleFromGroupAsync(string tenant, Guid clientId, Guid groupId, Guid roleId, string roleName);
    }
}

using TokenManager.Common.Models;
using TokenManager.Domain.Entities;

namespace TokenManager.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<Result<TokenDetails>> GetAccessTokenAsync(string tenant);
        Task<Result<TokenDetails>> LoginAsync(string tenant, User user);
        Task<Result<bool>> SignoutAsync(string tenant, string refreshToken);
        Task<Result<TokenDetails>> RefreshTokenAsync(string tenant, string refreshToken);
    }
}
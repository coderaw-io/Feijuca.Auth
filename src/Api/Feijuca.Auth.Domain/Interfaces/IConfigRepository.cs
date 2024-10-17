using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Models;

namespace Feijuca.Auth.Domain.Interfaces
{
    public interface IConfigRepository
    {
        Task<bool> AddConfigAsync(AuthSettings newConfig);
        AuthSettings GetConfig();
    }
}

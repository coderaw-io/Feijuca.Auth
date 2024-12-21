using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Entities;

namespace Feijuca.Auth.Domain.Interfaces
{
    public interface IConfigRepository
    {
        Task<bool> AddConfigAsync(KeycloakSettingsEntity newConfig, CancellationToken cancellationToken);
        Task<KeycloakSettingsEntity> GetConfigAsync();
        Task<bool> UpdateRealmConfigAsync(Guid id, KeycloakSettingsEntity keycloakSettings);
    }
}

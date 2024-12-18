using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Entities;

namespace Feijuca.Auth.Application.Mappers
{
    public static class ConfigMapper
    {
        public static KeycloakSettingsEntity ToEntity(this KeycloakSettings KeycloakSettings)
        {
            return new KeycloakSettingsEntity
            {
                Id = Guid.NewGuid(),
                Client = KeycloakSettings.Client,
                Realms = KeycloakSettings.Realms,
                Secrets = KeycloakSettings.Secrets,
                ServerSettings = KeycloakSettings.ServerSettings
            };
        }

        public static KeycloakSettings ToResponse(this KeycloakSettingsEntity KeycloakSettings)
        {
            return new KeycloakSettings
            {
                Client = KeycloakSettings.Client,
                Realms = KeycloakSettings.Realms,
                Secrets = KeycloakSettings.Secrets,
                ServerSettings = KeycloakSettings.ServerSettings
            };
        }
    }
}

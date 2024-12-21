using Feijuca.Auth.Models;

namespace Feijuca.Auth.Application.Requests.Config
{
    public record AddKeycloakSettingsRequest(Models.Client Client, Secrets Secrets, ServerSettings ServerSettings, Models.Realm Realm);
}

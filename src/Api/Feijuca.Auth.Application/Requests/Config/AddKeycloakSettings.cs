using Feijuca.Auth.Models;

namespace Feijuca.Auth.Application.Requests.Config
{
    public record AddKeycloakSettings(Models.Client Client, Secrets Secrets, ServerSettings ServerSettings);
}

using Feijuca.Auth.Models;

namespace Feijuca.Auth.Application.Requests.Config
{
    public record AddKeycloakSettings(Client Client, Secrets Secrets, ServerSettings ServerSettings);
}

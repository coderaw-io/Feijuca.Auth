using Feijuca.Auth.Models;

namespace Feijuca.Auth.Common.Models
{
    public class KeycloakSettings
    {
        public required Client Client { get; set; }
        public required Secrets Secrets { get; set; }
        public required ServerSettings ServerSettings { get; set; }
        public IEnumerable<Realm>? Realms { get; set; }
    }
}

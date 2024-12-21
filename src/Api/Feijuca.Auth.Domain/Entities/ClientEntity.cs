namespace Feijuca.Auth.Domain.Entities
{
    public class ClientEntity()
    {
        public string Id { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Secret { get; set; } = null!;
        public IEnumerable<string> RedirectUrls { get; set; } = [];
        public bool Enabled { get; set; }
    }
}

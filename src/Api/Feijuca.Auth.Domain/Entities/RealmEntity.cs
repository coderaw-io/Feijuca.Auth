namespace Feijuca.Auth.Domain.Entities
{
    public class RealmEntity
    {
        public required string Realm { get; set; }
        public string? DisplayName { get; set; }
        public bool Enabled { get; set; }
        public bool DefaultSwaggerTokenGeneration { get; set; }
    }
}

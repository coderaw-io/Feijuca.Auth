namespace Feijuca.Auth.Domain.Entities
{
    public class ClientScopeEntity
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Protocol { get; set; }
        public required Dictionary<string, string> Attributes { get; set; }
        public required List<ProtocolMapperEntity> ProtocolMappers { get; set; }
    }

    public class ProtocolMapperEntity
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Protocol { get; set; }
        public required string ProtocolMapperType { get; set; }
        public bool ConsentRequired { get; set; }
        public required Dictionary<string, string> Config { get; set; }
    }
}

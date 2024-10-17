namespace Feijuca.Auth.Infra.CrossCutting.Config
{
    public class MongoSettings
    {
        public required string Database { get; set; }
        public required string ConnectionString { get; set; }
    }
}

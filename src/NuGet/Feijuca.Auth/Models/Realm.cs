using System.Text.Json.Serialization;

namespace Feijuca.Auth.Models;

public class Realm
{
    [JsonIgnore]
    public required string ClientId { get; init; }

    [JsonIgnore]
    public required string ClientSecret { get; init; }

    public required string Name { get; init; }    

    public required string Audience { get; init; }

    public required string Issuer { get; init; }

    public bool DefaultSwaggerTokenGeneration { get; init; }
}

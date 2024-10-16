﻿namespace Feijuca.Auth.Models
{
    public class AuthSettings
    {
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
        public required string AuthServerUrl { get; set; }
        public required IEnumerable<Realm> Realms { get; set; }
        public string? PolicyName { get; set; }
        public IEnumerable<string>? Roles { get; set; } = [];
        public IEnumerable<string>? Scopes { get; set; } = [];
    }

    public class Realm
    {
        public string? Name { get; set; }
        public string? Audience { get; set; }
        public string? Issuer { get; set; }
        public bool UseAsDefaultSwaggerTokenGeneration { get; set; }
    }
}

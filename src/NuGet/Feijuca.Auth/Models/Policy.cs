﻿namespace Feijuca.Auth.Models
{
    public class Policy
    {
        public required string Name { get; init; }
        public required IEnumerable<string> Roles { get; init; }
    }
}
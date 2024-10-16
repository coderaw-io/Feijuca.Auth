﻿namespace Feijuca.Auth.Domain.Entities
{
    public class Role
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool Composite { get; set; }
        public bool ClientRole { get; set; }
        public string? ContainerId { get; set; }
    }
}

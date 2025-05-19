﻿using Feijuca.Auth.Models;

using Newtonsoft.Json;

namespace Feijuca.Auth.Domain.Entities
{
    public class User
    {
        [JsonIgnore]
        public string Password { get; set; }
        public Guid Id { get; set; }
        public bool Enabled { get; set; }
        public bool EmailVerified { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool Totp { get; set; }
        public List<string> DisableableCredentialTypes { get; set; } = [];
        public List<string> RequiredActions { get; set; } = [];
        public int NotBefore { get; set; }
        public long CreatedTimestamp { get; set; }
        public Access? Access { get; set; }
        public Dictionary<string, string[]> Attributes { get; set; }

        public User()
        {
            Password = "";
            Username = "";
            Email = "";
            Attributes = [];
        }

        public User(string userName, string password)
        {
            Username = userName;
            Password = password;
            Attributes = [];
            Email = "";
        }

        public User(string userName, string password, string email, string firstName, string lastName, Dictionary<string, string[]> attributes)
        {
            Enabled = true;
            EmailVerified = true;
            Email = email;
            Password = password;
            FirstName = firstName;
            Username = userName;
            LastName = lastName;
            Attributes = attributes;
            CreatedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}

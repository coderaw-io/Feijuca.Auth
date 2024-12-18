using AutoFixture;
using Feijuca.Auth.Application.Validators;
using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Models;
using FluentAssertions;

namespace Feijuca.Auth.Api.UnitTests.Validators
{
    public class KeycloakSettingsValidatorTests
    {
        private readonly Fixture _fixture = new();
        private readonly KeycloakSettingsValidator _keycloakSettingsValidator = new();

        [Fact]
        public void Given_NullPropertiesInKeycloakSettings_WhenValidatingSettings_Then_ReturnsFailures()
        {
            // Arrange
            var invalidKeycloakSettings = new KeycloakSettings
            {
                Client = new Client { ClientId = null! },
                Secrets = new Secrets { ClientSecret = null! },
                ServerSettings = new ServerSettings { Url = null! },
                Realms = [
                    new Realm
                    {
                        Name = null!,
                        Audience = null!,
                        Issuer = null!
                    } ]
            };

            // Act
            var result = _keycloakSettingsValidator.Validate(invalidKeycloakSettings);

            // Assert
            result
               .IsValid
               .Should()
               .BeFalse();

            result.Errors.Should().Contain(e => e.ErrorMessage == $"The {nameof(KeycloakSettings.Client.ClientId)} field is required.");
            result.Errors.Should().Contain(e => e.ErrorMessage == $"The {nameof(KeycloakSettings.Secrets.ClientSecret)} field is required.");
            result.Errors.Should().Contain(e => e.ErrorMessage == $"The {nameof(KeycloakSettings.ServerSettings.Url)} field is required.");
        }

        [Fact]
        public void Given_EmptyPropertiesInKeycloakSettings_WhenValidatingSettings_Then_ReturnsFailures()
        {
            // Arrange
            var invalidKeycloakSettings = new KeycloakSettings
            {
                Client = new Client { ClientId = string.Empty },
                Secrets = new Secrets { ClientSecret = string.Empty },
                ServerSettings = new ServerSettings { Url = string.Empty },
                Realms = [
                    new Realm
                    {
                        Name = null!,
                        Audience = null!,
                        Issuer = null!
                    } ]
            };

            // Act
            var result = _keycloakSettingsValidator.Validate(invalidKeycloakSettings);

            // Assert
            result
               .IsValid
               .Should()
               .BeFalse();

            result.Errors.Should().Contain(e => e.ErrorMessage == $"The {nameof(KeycloakSettings.Client.ClientId)} field is required.");
            result.Errors.Should().Contain(e => e.ErrorMessage == $"The {nameof(KeycloakSettings.Secrets.ClientSecret)} field is required.");
            result.Errors.Should().Contain(e => e.ErrorMessage == $"The {nameof(KeycloakSettings.ServerSettings.Url)} field is required.");
        }

        [Fact]
        public void Given_ValidKeycloakSettings_WhenValidatingSettings_Then_ReturnsSuccess()
        {
            // Arrange
            var validKeycloakSettings = _fixture.Create<KeycloakSettings>();

            // Act
            var result = _keycloakSettingsValidator.Validate(validKeycloakSettings);

            // Assert
            result
                .IsValid
                .Should()
                .BeTrue();
        }
    }
}

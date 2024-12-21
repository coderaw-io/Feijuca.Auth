using Feijuca.Auth.Common.Models;
using FluentValidation;

namespace Feijuca.Auth.Application.Validators
{
    public class KeycloakSettingsValidator : AbstractValidator<KeycloakSettings>
    {
        public KeycloakSettingsValidator()
        {
            RuleFor(x => x.Client.MasterClientId)
                .NotNull()
                .NotEmpty()
                .WithMessage($"The {nameof(KeycloakSettings.Client.MasterClientId)} field is required.");

            RuleFor(x => x.Secrets.MasterClientSecret)
                .NotNull()
                .NotEmpty()
                .WithMessage($"The {nameof(KeycloakSettings.Secrets.MasterClientSecret)} field is required.");

            RuleFor(x => x.ServerSettings.Url)
                .NotNull()
                .NotEmpty()
                .WithMessage($"The {nameof(KeycloakSettings.ServerSettings.Url)} field is required.");
        }
    }
}

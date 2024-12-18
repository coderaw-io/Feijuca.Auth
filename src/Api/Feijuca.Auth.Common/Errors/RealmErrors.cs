using Feijuca.Auth.Common.Models;

namespace Feijuca.Auth.Common.Errors
{
    public static class RealmErrors
    {
        public static string TechnicalMessage { get; private set; } = "";

        public static Error CreateRealmError => new(
            "Realm.CreateRealmError",
            $"An error occurred while trying create a realm {TechnicalMessage}"
        );

        public static void SetTechnicalMessage(string technicalMessage)
        {
            TechnicalMessage = technicalMessage;
        }
    }
}

using Feijuca.Auth.Application.Requests.Realm;
using Feijuca.Auth.Domain.Entities;

namespace Feijuca.Auth.Application.Mappers
{
    public static class RealmMapper
    {
        public static RealmEntity ToRealmEntity(this AddRealmRequest addRealmRequest)
        {
            return new RealmEntity
            {
                Realm = addRealmRequest.Name,
                DisplayName = addRealmRequest.Description,
                Enabled = true,
                DefaultSwaggerTokenGeneration = addRealmRequest.DefaultSwaggerTokenGeneration
            };
        }
    }
}

namespace Feijuca.Auth.Application.Requests.Realm
{
    public record AddRealmRequest(string Name, string Description, bool DefaultSwaggerTokenGeneration);
}

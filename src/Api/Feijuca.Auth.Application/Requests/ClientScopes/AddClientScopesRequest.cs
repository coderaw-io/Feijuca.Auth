namespace Feijuca.Auth.Application.Requests.ClientScopes
{
    public record AddClientScopesRequest(string Name, string Description, bool IncludeInTokenScope);
}

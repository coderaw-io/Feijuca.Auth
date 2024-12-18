namespace Feijuca.Auth.Domain.Entities
{
    public record ClientScopesEntity(string Name, string Description, bool IncludeInTokenScope);
}

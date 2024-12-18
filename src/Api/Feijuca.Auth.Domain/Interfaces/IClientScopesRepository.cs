namespace Feijuca.Auth.Domain.Interfaces
{
    public interface IClientScopesRepository : IBaseRepository
    {
        Task<bool> AddClientScopes();
    }
}

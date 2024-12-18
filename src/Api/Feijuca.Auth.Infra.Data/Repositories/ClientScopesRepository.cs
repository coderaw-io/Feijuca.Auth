using Feijuca.Auth.Domain.Interfaces;

namespace Feijuca.Auth.Infra.Data.Repositories
{
    public class ClientScopesRepository(IHttpClientFactory httpClientFactory) : BaseRepository(httpClientFactory), IClientScopesRepository
    {
    }
}

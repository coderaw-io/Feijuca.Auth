using Feijuca.Auth.Domain.Entities;
using Feijuca.Auth.Domain.Interfaces;
using Flurl;
using Newtonsoft.Json;
using System.Text;

namespace Feijuca.Auth.Infra.Data.Repositories
{
    public class ClientScopesRepository(IHttpClientFactory httpClientFactory, IAuthRepository authRepository, ITenantService tenantService) 
        : BaseRepository(httpClientFactory), IClientScopesRepository
    {
        public async Task<bool> AddClientScopesAsync(ClientScopesEntity clientScopesEntity, CancellationToken cancellationToken)
        {
            var tokenDetails = await authRepository.GetAccessTokenAsync(cancellationToken);
            using var httpClient = CreateHttpClientWithHeaders(tokenDetails.Response.Access_Token);

            var url = httpClient.BaseAddress
                .AppendPathSegment("admin")
                .AppendPathSegment("realms")
                .AppendPathSegment(tenantService.Tenant)
                .AppendPathSegment("client-scopes");

            var clientScope = new
            {
                name = clientScopesEntity.Name,
                description = clientScopesEntity.Description,
                protocol = "openid-connect",
                attributes = new Dictionary<string, bool>
                {
                    { "display.on.consent.screen", true },
                    { "include.in.token.scope", clientScopesEntity.IncludeInTokenScope }
                },
                defaultScope = true
            };

            var jsonContent = JsonConvert.SerializeObject(clientScope);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            using var response = await httpClient.PostAsync(url, content, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> AddClientScopeToClientAsync(string clientId, string clientScopeId, CancellationToken cancellationToken)
        {
            var tokenDetails = await authRepository.GetAccessTokenAsync(cancellationToken);
            using var httpClient = CreateHttpClientWithHeaders(tokenDetails.Response.Access_Token);

            var url = httpClient.BaseAddress
                .AppendPathSegment("admin")
                .AppendPathSegment("realms")
                .AppendPathSegment(tenantService.Tenant)
                .AppendPathSegment("clients")
                .AppendPathSegment(clientId)
                .AppendPathSegment("default-client-scopes")
                .AppendPathSegment(clientScopeId);

            using var response = await httpClient.PutAsync(url, default, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<ClientScopeEntity>> GetClientScopesAsync(CancellationToken cancellationToken)
        {
            var tokenDetails = await authRepository.GetAccessTokenAsync(cancellationToken);
            using var httpClient = CreateHttpClientWithHeaders(tokenDetails.Response.Access_Token);

            var url = httpClient.BaseAddress
                .AppendPathSegment("admin")
                .AppendPathSegment("realms")
                .AppendPathSegment(tenantService.Tenant)
                .AppendPathSegment("client-scopes");

            using var response = await httpClient.GetAsync(url, cancellationToken);
            var scopesList = new List<ClientScopeEntity>();

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
                var clientScopes = JsonConvert.DeserializeObject<IEnumerable<ClientScopeEntity>>(responseBody);

                return clientScopes!;
            }

            return scopesList;

        }
    }
}

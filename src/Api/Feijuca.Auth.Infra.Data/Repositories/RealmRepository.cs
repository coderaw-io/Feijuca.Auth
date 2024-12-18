using Feijuca.Auth.Domain.Entities;
using Feijuca.Auth.Domain.Interfaces;
using Flurl;
using Newtonsoft.Json;
using System.Text;

namespace Feijuca.Auth.Infra.Data.Repositories
{
    public class RealmRepository(IHttpClientFactory httpClientFactory, IAuthRepository authRepository) : BaseRepository(httpClientFactory), IRealmRepository
    {
        private readonly IAuthRepository _authRepository = authRepository;

        public async Task<bool> CreateRealmAsync(RealmEntity realm, CancellationToken cancellationToken)
        {
            var tokenDetails = await _authRepository.GetAccessTokenAsync(cancellationToken);
            using var httpClient = CreateHttpClientWithHeaders(tokenDetails.Response.Access_Token);

            var url = httpClient.BaseAddress
                   .AppendPathSegment("admin")
                   .AppendPathSegment("realms");

            var realmBody = new
            {
                realm = realm.Realm,
                enabled = realm.Enabled,
                displayName = realm.DisplayName
            };

            var jsonContent = JsonConvert.SerializeObject(realmBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var resut = await httpClient.PostAsync(url, content, cancellationToken);
            if (resut.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<string> GetRealmConfigAsync(string name, CancellationToken cancellationToken)
        {
            var tokenDetails = await _authRepository.GetAccessTokenAsync(cancellationToken);
            var httpClient = CreateHttpClientWithHeaders(tokenDetails.Response.Access_Token);

            var url = httpClient.BaseAddress
                    .AppendPathSegment("admin")
                    .AppendPathSegment("realms")
                    .AppendPathSegment(name)
                    .AppendPathSegment("clients");

            var response = await httpClient.GetAsync(url, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                var result = JsonConvert.DeserializeObject<string>(responseContent)!;
                return result;
            }

            return "";
        }
    }
}

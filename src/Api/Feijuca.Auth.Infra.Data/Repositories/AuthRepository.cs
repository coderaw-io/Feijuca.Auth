using Feijuca.Auth.Common.Errors;
using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Entities;
using Feijuca.Auth.Domain.Interfaces;
using Flurl;
using Newtonsoft.Json;

namespace Feijuca.Auth.Infra.Data.Repositories
{
    public class AuthRepository(TokenCredentials tokenCredentials, IHttpClientFactory httpClientFactory) : IAuthRepository
    {
        private readonly TokenCredentials _tokenCredentials = tokenCredentials;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<Result<TokenDetails>> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            var requestData = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", _tokenCredentials.Client_Id),
                new KeyValuePair<string, string>("client_secret", _tokenCredentials.Client_Secret),
            ]);

            using var httpClient = _httpClientFactory.CreateClient("KeycloakClient");
            var url = httpClient.BaseAddress
                .AppendPathSegment("realms")
                .AppendPathSegment("master")
                .AppendPathSegment("protocol")
                .AppendPathSegment("openid-connect")
                .AppendPathSegment("token");

            using var response =  await httpClient.PostAsync(url, requestData, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(cancellationToken);
                var result = JsonConvert.DeserializeObject<TokenDetails>(content)!;

                return Result<TokenDetails>.Success(result);
            }

            var responseMessage = await response.Content.ReadAsStringAsync(cancellationToken);
            UserErrors.SetTechnicalMessage(responseMessage);
            return Result<TokenDetails>.Failure(UserErrors.TokenGenerationError);
        }
    }
}

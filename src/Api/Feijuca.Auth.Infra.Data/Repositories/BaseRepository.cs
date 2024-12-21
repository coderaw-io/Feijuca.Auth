using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Interfaces;
using System.Net.Http.Headers;
using System.Threading;

namespace Feijuca.Auth.Infra.Data.Repositories
{
    public class BaseRepository(IHttpClientFactory httpClientFactory) : IBaseRepository
    {
        public HttpClient CreateHttpClientWithHeaders(string accessToken)
        {
            var httpClient = httpClientFactory.CreateClient("KeycloakClient");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return httpClient;
        }
    }
}

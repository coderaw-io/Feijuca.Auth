using Feijuca.Auth.Common.Errors;
using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Entities;
using Feijuca.Auth.Domain.Interfaces;
using Flurl;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;

namespace Feijuca.Auth.Infra.Data.Repositories
{
    public class GroupRolesRepository(IHttpClientFactory httpClientFactory, IAuthRepository authRepository, ITenantService tenantService)
        : BaseRepository(httpClientFactory), IGroupRolesRepository
    {
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly ITenantService _tenantService = tenantService;

        public async Task<Result<bool>> AddRoleToGroupAsync(string groupId, string clientId, Guid roleId, string roleName, CancellationToken cancellationToken)
        {
            var tokenDetails = await _authRepository.GetAccessTokenAsync(cancellationToken);
            using var httpClient = CreateHttpClientWithHeaders(tokenDetails.Response.Access_Token);

            var url = httpClient.BaseAddress
                    .AppendPathSegment("admin")
                    .AppendPathSegment("realms")
                    .AppendPathSegment(_tenantService.Tenant)
                    .AppendPathSegment("groups")
                    .AppendPathSegment(groupId)
                    .AppendPathSegment("role-mappings")
                    .AppendPathSegment("clients")
                    .AppendPathSegment(clientId);

            var roleData = new[]
            {
                new { id = roleId, name = roleName }
            };

            var content = new StringContent(JsonConvert.SerializeObject(roleData), Encoding.UTF8, "application/json");

            using var response = await httpClient.PostAsync(url, content, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(GroupRolesErrors.ErrorAddRoleToGroup);
        }

        public async Task<Result<IEnumerable<ClientMapping>>> GetGroupRolesAsync(string groupId, CancellationToken cancellationToken)
        {
            var tokenDetails = await _authRepository.GetAccessTokenAsync(cancellationToken);
            using var httpClient = CreateHttpClientWithHeaders(tokenDetails.Response.Access_Token);

            var url = httpClient.BaseAddress
                    .AppendPathSegment("admin")
                    .AppendPathSegment("realms")
                    .AppendPathSegment(_tenantService.Tenant)
                    .AppendPathSegment("groups")
                    .AppendPathSegment(groupId)
                    .AppendPathSegment("role-mappings");

            using var response = await httpClient.GetAsync(url, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var groupRolesContent = await response.Content.ReadAsStringAsync(cancellationToken);
                var jsonObject = JObject.Parse(groupRolesContent);
                var clientMappings = jsonObject["clientMappings"]!
                    .Children<JProperty>()
                    .Select(x => x.Value.ToObject<ClientMapping>())
                    .ToList();

                return Result<IEnumerable<ClientMapping>>.Success(clientMappings!);
            }

            return Result<IEnumerable<ClientMapping>>.Failure(GroupRolesErrors.ErrorGetGroupRoles);
        }

        public async Task<Result> RemoveRoleFromGroupAsync(string clientId, string groupId, Guid roleId, string roleName, CancellationToken cancellationToken)
        {
            var tokenDetails = await _authRepository.GetAccessTokenAsync(cancellationToken);
            using var httpClient = CreateHttpClientWithHeaders(tokenDetails.Response.Access_Token);

            var url = httpClient.BaseAddress
                    .AppendPathSegment("admin")
                    .AppendPathSegment("realms")
                    .AppendPathSegment(_tenantService.Tenant)
                    .AppendPathSegment("groups")
                    .AppendPathSegment(groupId)
                    .AppendPathSegment("role-mappings")
                    .AppendPathSegment("clients")
                    .AppendPathSegment(clientId);

            var body = JsonConvert.SerializeObject(new[]
            {
                new {
                    id = roleId,
                    name = roleName
                }
            });

            var request = new HttpRequestMessage(HttpMethod.Delete, url)
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };

            using var response = await httpClient.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(GroupRolesErrors.RemovingRoleFromGroupError);
        }
    }
}

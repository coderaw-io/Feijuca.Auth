using Feijuca.Auth.Common.Errors;
using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Interfaces;
using Flurl;

namespace Feijuca.Auth.Infra.Data.Repositories
{
    public class UserGroupRepository(IHttpClientFactory httpClientFactory, IAuthRepository authRepository, ITenantService tenantService) : BaseRepository(httpClientFactory), IGroupUsersRepository
    {
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly ITenantService _tenantService = tenantService;

        public async Task<Result<bool>> AddUserToGroupAsync(Guid userId, Guid groupId, CancellationToken cancellationToken)
        {
            var tokenDetails = await _authRepository.GetAccessTokenAsync(cancellationToken);
            using var httpClient = CreateHttpClientWithHeaders(tokenDetails.Response.Access_Token);

            var url = httpClient.BaseAddress
                    .AppendPathSegment("admin")
                    .AppendPathSegment("realms")
                    .AppendPathSegment(_tenantService.Tenant)
                    .AppendPathSegment("users")
                    .AppendPathSegment(userId)
                    .AppendPathSegment("groups")
                    .AppendPathSegment(groupId);

            var response = await httpClient.PutAsync(url, default, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(UserGroupErrors.ErrorAddUserToGroup);
        }

        public async Task<Result<bool>> RemoveUserFromGroupAsync(Guid userId, Guid groupId, CancellationToken cancellationToken)
        {
            var tokenDetails = await _authRepository.GetAccessTokenAsync(cancellationToken);
            using var httpClient = CreateHttpClientWithHeaders(tokenDetails.Response.Access_Token);

            var url = httpClient.BaseAddress
                    .AppendPathSegment("admin")
                    .AppendPathSegment("realms")
                    .AppendPathSegment(_tenantService.Tenant)
                    .AppendPathSegment("users")
                    .AppendPathSegment(userId)
                    .AppendPathSegment("groups")
                    .AppendPathSegment(groupId);

            using var response = await httpClient.DeleteAsync(url, default);

            if (response.IsSuccessStatusCode)
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(UserGroupErrors.ErrorAddUserToGroup);
        }
    }
}

﻿using Feijuca.Auth.Common.Errors;
using Feijuca.Auth.Common.Extensions;
using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Entities;
using Feijuca.Auth.Domain.Filters;
using Feijuca.Auth.Domain.Interfaces;
using Flurl;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Feijuca.Auth.Infra.Data.Repositories
{
    public class GroupRepository(IHttpClientFactory httpClientFactory, IAuthRepository authRepository, ITenantService tenantService) : IGroupRepository
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IAuthRepository _authRepository = authRepository;
        private readonly ITenantService _tenantService = tenantService;

        public async Task<Result<IEnumerable<Group>>> GetAllAsync(CancellationToken cancellationToken)
        {
            var tokenDetailsResult = await _authRepository.GetAccessTokenAsync(cancellationToken);

            if (tokenDetailsResult.IsSuccess)
            {
                var httpClient = CreateHttpClientWithHeaders(tokenDetailsResult.Response.Access_Token);

                var url = httpClient.BaseAddress
                        .AppendPathSegment("admin")
                        .AppendPathSegment("realms")
                        .AppendPathSegment(_tenantService.Tenant)
                        .AppendPathSegment("groups");

                var response = await httpClient.GetAsync(url, cancellationToken);
                var groups = await response.Content.ReadAsStringAsync(cancellationToken);
                var users = JsonConvert.DeserializeObject<IEnumerable<Group>>(groups)!;

                return Result<IEnumerable<Group>>.Success(users);
            }

            return Result<IEnumerable<Group>>.Failure(tokenDetailsResult.Error);
        }

        public async Task<Result> CreateAsync(string name, Dictionary<string, string[]> attributes, CancellationToken cancellationToken)
        {
            var tokenDetails = await _authRepository.GetAccessTokenAsync(cancellationToken);
            var httpClient = CreateHttpClientWithHeaders(tokenDetails.Response.Access_Token);

            var url = httpClient.BaseAddress
                    .AppendPathSegment("admin")
                    .AppendPathSegment("realms")
                    .AppendPathSegment(_tenantService.Tenant)
                    .AppendPathSegment("groups");

            var group = new
            {
                name,
                attributes
            };

            var jsonContent = JsonConvert.SerializeObject(group);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return Result.Success();
            }

            GroupErrors.SetTechnicalMessage(response.ReasonPhrase!, cancellationToken);
            return Result.Failure(GroupErrors.CreationGroupError);
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var tokenDetails = await _authRepository.GetAccessTokenAsync(cancellationToken);
            var httpClient = CreateHttpClientWithHeaders(tokenDetails.Response.Access_Token);

            var url = httpClient.BaseAddress
                    .AppendPathSegment("admin")
                    .AppendPathSegment("realms")
                    .AppendPathSegment(_tenantService.Tenant)
                    .AppendPathSegment("groups")
                    .AppendPathSegment(id);

            var response = await httpClient.DeleteAsync(url, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return Result.Success();
            }

            GroupErrors.SetTechnicalMessage(response.ReasonPhrase!, cancellationToken);
            return Result.Failure(GroupErrors.DeletionGroupError);
        }

        public async Task<Result<IEnumerable<User>>> GetUsersInGroupAsync(Guid id, UserFilters userFilters, CancellationToken cancellationToken)
        {
            var tokenDetails = await _authRepository.GetAccessTokenAsync(cancellationToken);
            var httpClient = CreateHttpClientWithHeaders(tokenDetails.Response.Access_Token);
            int first = (userFilters.PageFilter.PageNumber - 1) * userFilters.PageFilter.PageSize;

            var url = httpClient.BaseAddress
                    .AppendPathSegment("admin")
                    .AppendPathSegment("realms")
                    .AppendPathSegment(_tenantService.Tenant)
                    .AppendPathSegment("groups")
                    .AppendPathSegment(id)
                    .AppendPathSegment("members")
                    .SetQueryParam("first", first)
                    .SetQueryParam("max", userFilters.PageFilter.PageSize)
                    .SetCollectionQueryParam("username", userFilters.Emails);

            var response = await httpClient.GetAsync(url, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
                var users = JsonConvert.DeserializeObject<IEnumerable<User>>(responseContent);

                return Result<IEnumerable<User>>.Success(users!);
            }

            return Result<IEnumerable<User>>.Failure(GroupErrors.GetUsersInGroupsError);
        }

        private HttpClient CreateHttpClientWithHeaders(string accessToken)
        {
            var httpClient = _httpClientFactory.CreateClient("KeycloakClient");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return httpClient;
        }

    }
}

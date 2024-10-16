﻿using Feijuca.Auth.Common.Errors;
using Feijuca.Auth.Common.Models;

using Feijuca.Auth.Application.Mappers;
using Feijuca.Auth.Application.Responses;
using Feijuca.Auth.Domain.Interfaces;

using MediatR;

namespace Feijuca.Auth.Application.Queries.Users
{
    public class GetUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUsersQuery, Result<PagedResult<UserResponse>>>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Result<PagedResult<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetAllAsync(request.GetUsersRequest.ToUserFilters());

            if (result.IsSuccess)
            {
                var totalUsers = await _userRepository.GetTotalAsync();
                return Result<PagedResult<UserResponse>>.Success(result.Response.ToUserResponse(request.GetUsersRequest.PageFilter, totalUsers));
            }

            return Result<PagedResult<UserResponse>>.Failure(UserErrors.GetAllUsersError);
        }
    }
}
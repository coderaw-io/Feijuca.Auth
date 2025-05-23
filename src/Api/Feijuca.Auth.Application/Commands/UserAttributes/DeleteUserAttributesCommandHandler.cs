﻿using Feijuca.Auth.Common.Errors;
using Feijuca.Auth.Common.Models;
using Feijuca.Auth.Domain.Interfaces;
using Feijuca.Auth.Services;
using MediatR;

namespace Feijuca.Auth.Application.Commands.UserAttributes
{
    public class DeleteUserAttributesCommandHandler(IUserRepository userRepository, ITenantService tenantService) : IRequestHandler<DeleteUserAttributesCommand, Result<bool>>
    {
        public async Task<Result<bool>> Handle(DeleteUserAttributesCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetAsync(tenantService.Tenant.Name, request.Username, cancellationToken);

            if (user?.Response == null)
            {
                return Result<bool>.Failure(UserErrors.ErrorWhileAddedUserAttribute);
            }

            foreach (var key in request.DeleteUserAttributeRequest)
            {
                user.Response.Attributes.Remove(key);
            }

            var result = await userRepository.UpdateUserAsync(user.Response.Id, user.Response, cancellationToken);

            if (result.IsSuccess)
            {
                return Result<bool>.Success(true);
            }

            return Result<bool>.Failure(UserErrors.ErrorWhileAddedUserAttribute);
        }

    }
}

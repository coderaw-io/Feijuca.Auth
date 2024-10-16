﻿using Feijuca.Auth.Application.Commands.Auth;
using Feijuca.Auth.Application.Commands.Users;
using Feijuca.Auth.Application.Queries.Users;
using Feijuca.Auth.Application.Requests.Auth;
using Feijuca.Auth.Application.Requests.User;
using Feijuca.Auth.Attributes;
using Feijuca.Auth.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feijuca.Auth.Api.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Get all users existing on the Keycloak realm.
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpGet]
        [Route("{tenant}/users", Name = nameof(GetUsers))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [RequiredRole("Feijuca.ApiReader")]
        public async Task<IActionResult> GetUsers([FromRoute] string tenant, [FromQuery] GetUsersRequest getUsersRequest, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUsersQuery(tenant, getUsersRequest), cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result.Response);
            }

            var responseError = Result<string>.Failure(result.Error);
            return BadRequest(responseError);
        }

        /// <summary>
        /// Add a new user on the Keycloak realm.
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpPost]
        [Route("{tenant}/user", Name = nameof(CreateUser))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiWriter")]
        [Authorize]
        public async Task<IActionResult> CreateUser([FromRoute] string tenant, [FromBody] AddUserRequest addUserRequest, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateUserCommand(tenant, addUserRequest), cancellationToken);

            if (result.IsSuccess)
            {
                var response = Result<string>.Success("User created successfully");
                return Created($"/api/v1/users/{tenant}", response.Response);
            }

            var responseError = Result<string>.Failure(result.Error);
            return BadRequest(responseError.Error);
        }

        /// <summary>
        /// Delete an existing user on the Keycloak realm.
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpDelete]
        [Route("{tenant}/user/{id}", Name = nameof(DeleteUser))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiWriter")]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromRoute] string tenant, [FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteUserCommand(tenant, id), cancellationToken);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            var responseError = Result<string>.Failure(result.Error);
            return BadRequest(responseError);
        }


        /// <summary>
        /// Logout a user and invalidate the session.
        /// </summary>
        /// <returns>A status code related to the operation.</returns>
        [HttpPost]
        [Route("{tenant}/user/logout", Name = nameof(Logout))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout([FromRoute] string tenant, [FromBody] RefreshTokenRequest logoutUserRequest, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new SignoutCommand(tenant, logoutUserRequest.RefreshToken), cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(new { Message = "Logout successful" });
            }

            var responseError = Result<string>.Failure(result.Error);
            return BadRequest(responseError);
        }
    }
}
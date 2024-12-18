using Feijuca.Auth.Application.Commands.GroupUser;
using Feijuca.Auth.Application.Queries.GroupUser;
using Feijuca.Auth.Application.Requests.GroupUsers;
using Feijuca.Auth.Attributes;
using Feijuca.Auth.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feijuca.Auth.Api.Controllers
{
    [Route("api/v1")]
    [ApiController]
    [Authorize]
    public class GroupsUsersController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Adds a user to a specific group in the specified Keycloak realm.
        /// </summary>
        /// <returns>
        /// A 204 No Content status code if the user is successfully added to the group; 
        /// otherwise, a 400 Bad Request status code with an error message.
        /// </returns>
        /// <param name="addUserToGroupRequest">An object of type <see cref="T:Feijuca.Auth.Common.Models.AddUserToGroupRequest"/> containing the user and group IDs for the operation.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> used to observe cancellation requests for the operation.</param>
        /// <response code="204">The user was successfully added to the group.</response>
        /// <response code="400">The request was invalid or could not be processed.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [HttpPost("/group/user", Name = nameof(AddUserToGroup))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiWriter")]
        public async Task<IActionResult> AddUserToGroup([FromBody] AddUserToGroupRequest addUserToGroupRequest,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AddUserToGroupCommand(addUserToGroupRequest.UserId, addUserToGroupRequest.GroupId), cancellationToken);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            var responseError = Result<string>.Failure(result.Error);
            return BadRequest(responseError);
        }

        /// <summary>
        /// Retrieves all users present in a specific group within the specified Keycloak realm.
        /// </summary>
        /// <returns>
        /// A 200 OK status code with a list of users in the group; 
        /// otherwise, a 400 Bad Request status code with an error message.
        /// </returns>
        /// <param name="usersGroup">An object of type <see cref="T:Feijuca.Auth.Common.Models.GetUsersGroupRequest"/> containing the necessary parameters to filter users in the group.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> used to observe cancellation requests for the operation.</param>
        /// <response code="200">The users in the group were successfully retrieved.</response>
        /// <response code="400">The request was invalid or could not be processed.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [HttpGet("/group/user", Name = nameof(GetUsersInGroup))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiReader")]
        public async Task<IActionResult> GetUsersInGroup([FromQuery] GetUsersGroupRequest usersGroup, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetUsersGroupQuery(usersGroup), cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result.Response);
            }

            var responseError = Result<string>.Failure(result.Error);
            return BadRequest(responseError);
        }

        /// <summary>
        /// Removes a user from a specific group within the specified Keycloak realm.
        /// </summary>
        /// <returns>
        /// A 204 No Content status code if the user is successfully removed from the group; 
        /// otherwise, a 400 Bad Request status code with an error message.
        /// </returns>
        /// <param name="removeUserFromGroup">An object of type <see cref="T:Feijuca.Auth.Common.Models.RemoveUserFromGroupRequest"/> containing the user ID and group ID for the operation.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> used to observe cancellation requests for the operation.</param>
        /// <response code="204">The user was successfully removed from the group.</response>
        /// <response code="400">The request was invalid or could not be processed.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [HttpDelete("/group/user", Name = nameof(RemoveUserFromGroup))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiWriter")]
        public async Task<IActionResult> RemoveUserFromGroup([FromBody] RemoveUserFromGroupRequest removeUserFromGroup, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new RemoveUserFromGroupCommand(removeUserFromGroup.UserId, removeUserFromGroup.GroupId), cancellationToken);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            var responseError = Result<string>.Failure(result.Error);
            return BadRequest(responseError);
        }
    }
}

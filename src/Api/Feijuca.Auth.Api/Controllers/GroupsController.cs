using Feijuca.Auth.Application.Commands.Group;
using Feijuca.Auth.Application.Queries.Groups;
using Feijuca.Auth.Application.Requests.User;
using Feijuca.Auth.Attributes;
using Feijuca.Auth.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feijuca.Auth.Api.Controllers
{
    [Route("api/v1/groups")]
    [ApiController]
    [Authorize]
    public class GroupsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Returns all groups registered in the realm
        /// </summary>
        /// <returns>
        /// A 200 OK status code along with the list of groups if the operation is successful;
        /// otherwise, a 400 Bad Request status code with an error message, or a 500 Internal Server Error status code if something goes wrong.
        /// </returns>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> used to observe cancellation requests for the operation.</param>
        /// <response code="200">The operation was successful, and the list of groups is returned.</response>
        /// <response code="400">The request was invalid or could not be processed.</response>
        /// <response code="500">An internal server error occurred during the processing of the request.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiReader")]
        public async Task<IActionResult> GetGroups(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllGroupsQuery(), cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result.Response);
            }

            var responseError = Result<string>.Failure(result.Error);
            return BadRequest(responseError);
        }

        /// <summary>
        /// Deletes an existing group from the specified Keycloak realm.
        /// </summary>
        /// <returns>
        /// A 204 No Content status code if the group was successfully deleted;
        /// otherwise, a 400 Bad Request status code with an error message, or a 500 Internal Server Error status code if something goes wrong.
        /// </returns>
        /// <param name="id">The unique identifier of the group to be deleted.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> used to observe cancellation requests for the operation.</param>
        /// <response code="204">The group was successfully deleted.</response>
        /// <response code="400">The request was invalid or could not be processed.</response>
        /// <response code="500">An internal server error occurred during the processing of the request.</response>
        [HttpDelete]
        [Route("{id}", Name = nameof(DeleteGroup))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiWriter")]
        public async Task<IActionResult> DeleteGroup([FromRoute] string id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new DeleteGroupCommand(id), cancellationToken);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            var responseError = Result<string>.Failure(result.Error);
            return BadRequest(responseError);
        }

        /// <summary>
        /// Adds a new group to the specified Keycloak realm.
        /// </summary>
        /// <returns>
        /// A 201 Created status code along with a success message if the group is successfully created;
        /// otherwise, a 400 Bad Request status code with an error message, or a 500 Internal Server Error status code if something goes wrong.
        /// </returns>
        /// <param name="addGroupRequest">An object of type <see cref="T:Feijuca.Auth.Common.Models.AddGroupRequest"/> containing the details of the group to be created.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> used to observe cancellation requests for the operation.</param>
        /// <response code="201">The group was successfully created.</response>
        /// <response code="400">The request was invalid or could not be processed.</response>
        /// <response code="500">An internal server error occurred during the processing of the request.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiWriter")]
        public async Task<IActionResult> CreateGroup([FromBody] AddGroupRequest addGroupRequest, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateGroupCommand(addGroupRequest), cancellationToken);

            if (result.IsSuccess)
            {
                var response = Result<string>.Success("Group created successfully");
                return Created("/createGroup", response);
            }

            var responseError = Result<string>.Failure(result.Error);
            return BadRequest(responseError);
        }
    }
}

using Feijuca.Auth.Application.Commands.GroupRoles;
using Feijuca.Auth.Application.Queries.GroupRoles;
using Feijuca.Auth.Application.Requests.GroupRoles;
using Feijuca.Auth.Attributes;
using Feijuca.Auth.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feijuca.Auth.Api.Controllers
{
    [Route("api/v1/groups-roles")]
    [ApiController]
    [Authorize]
    public class GroupsRolesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Adds a role to a specific group in the specified Keycloak realm.
        /// </summary>
        /// <returns>
        /// A 201 Created status code if the role is successfully added to the group;
        /// otherwise, a 400 Bad Request status code with an error message.
        /// </returns>
        /// <param name="id">The unique identifier of the group to which the role will be added.</param>
        /// <param name="addRoleToGroup">An object of type <see cref="T:Feijuca.Auth.Common.Models.RoleToGroupRequest"/> containing the details of the role to be added to the group.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> used to observe cancellation requests for the operation.</param>
        /// <response code="201">The role was successfully added to the group.</response>
        /// <response code="400">The request was invalid or could not be processed.</response>
        [HttpPost("{id:guid}/role", Name = nameof(AddRoleToGroup))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiWriter")]
        public async Task<IActionResult> AddRoleToGroup(
            [FromRoute] string id,
            [FromBody] RoleToGroupRequest addRoleToGroup,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AddRoleToGroupCommand(id, addRoleToGroup), cancellationToken);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetGroupRoles), new { id }, null);
            }

            return BadRequest(Result<string>.Failure(result.Error));
        }

        /// <summary>
        /// Removes a role from a specific group in the specified Keycloak realm.
        /// </summary>
        /// <returns>
        /// A 204 No Content status code if the role is successfully removed from the group;
        /// otherwise, a 400 Bad Request status code with an error message.
        /// </returns>
        /// <param name="groupid">The unique identifier of the group from which the role will be removed.</param>
        /// <param name="roleid">The unique identifier of the role to be removed from the group.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> used to observe cancellation requests for the operation.</param>
        /// <response code="204">The role was successfully removed from the group.</response>
        /// <response code="400">The request was invalid or could not be processed.</response>
        [HttpDelete("{groupid}/role/{roleid}", Name = nameof(RemoveRoleFromGroup))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiWriter")]
        public async Task<IActionResult> RemoveRoleFromGroup(
            [FromRoute] string groupid,
            [FromRoute] Guid roleid,
            CancellationToken cancellationToken)
        {
            var roleToGroupRequest = new RoleToGroupRequest(groupid, roleid);
            var result = await _mediator.Send(new RemoveRoleFromGroupCommand(groupid, roleToGroupRequest), cancellationToken);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            return BadRequest(Result<string>.Failure(result.Error));
        }

        /// <summary>
        /// Retrieves the roles associated with a specific group in the specified Keycloak realm.
        /// </summary>
        /// <returns>
        /// A 200 OK status code with a list of roles associated with the group;
        /// otherwise, a 400 Bad Request status code with an error message.
        /// </returns>
        /// <param name="id">The unique identifier of the group whose roles are being retrieved.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> used to observe cancellation requests for the operation.</param>
        /// <response code="200">The roles associated with the group were successfully retrieved.</response>
        /// <response code="400">The request was invalid or could not be processed.</response>
        [HttpGet("{id:guid}/roles", Name = nameof(GetGroupRoles))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiReader")]
        public async Task<IActionResult> GetGroupRoles(
            [FromRoute] string id,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetGroupRolesQuery(id), cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result.Response);
            }

            return BadRequest(Result<string>.Failure(result.Error));
        }
    }
}

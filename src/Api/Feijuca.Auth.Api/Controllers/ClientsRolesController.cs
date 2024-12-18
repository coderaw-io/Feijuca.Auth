using Feijuca.Auth.Application.Commands.ClientRole;
using Feijuca.Auth.Application.Queries.Permissions;
using Feijuca.Auth.Application.Requests.Role;
using Feijuca.Auth.Attributes;
using Feijuca.Auth.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feijuca.Auth.Api.Controllers
{
    [Route("api/v1/clients-roles")]
    [ApiController]
    [Authorize]
    public class ClientsRolesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Retrieves all roles associated with the clients in the specified Keycloak realm.
        /// </summary>
        /// <returns>
        /// A 200 OK status code containing a list of roles if the request is successful;
        /// otherwise, a 400 Bad Request status code with an error message.
        /// </returns>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> that can be used to signal cancellation for the operation.</param>
        /// <response code="200">A list of roles associated with the clients in the specified realm.</response>
        /// <response code="400">The request was invalid or could not be processed.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiReader")]
        public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetRolesQuery(), cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result.Response);
            }

            return BadRequest(Result<string>.Failure(result.Error));
        }

        /// <summary>
        /// Adds a new role to a client in the specified Keycloak realm.
        /// </summary>
        /// <param name="addRoleRequest">The request object containing the details of the role to be added.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> that can be used to signal cancellation for the operation.</param>
        /// <returns>
        /// A 201 Created status code if the role was successfully added;
        /// otherwise, a 400 Bad Request status code with an error message.
        /// </returns>
        /// <response code="201">The role was created successfully.</response>
        /// <response code="400">The request was invalid or could not be processed.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiWriter")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleRequest addRoleRequest, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AddClientRoleCommand(addRoleRequest), cancellationToken);

            if (result.IsSuccess)
            {
                return Created();
            }

            return BadRequest(Result<string>.Failure(result.Error));
        }
    }
}

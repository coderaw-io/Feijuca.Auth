﻿using Feijuca.Auth.Application.Commands.Client;
using Feijuca.Auth.Application.Commands.ClientScopes;
using Feijuca.Auth.Application.Queries.Clients;
using Feijuca.Auth.Application.Requests.Client;
using Feijuca.Auth.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feijuca.Auth.Api.Controllers
{
    [Route("api/v1/clients")]
    [ApiController]
    [Authorize]
    public class ClientsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Recovers all clients registered in the realm.
        /// </summary>
        /// <returns>
        /// A 200 OK status code along with the list of clients if the operation is successful;
        /// otherwise, a 400 Bad Request status code with an error message, or a 500 Internal Server Error status code if something goes wrong.
        /// </returns>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> used to observe cancellation requests for the operation.</param>
        /// <response code="200">The operation was successful, and the list of clients is returned.</response>
        /// <response code="400">The request was invalid or could not be processed.</response>
        /// <response code="500">An internal server error occurred during the processing of the request.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiReader")]        
        public async Task<IActionResult> GetClients(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllClientsQuery(), cancellationToken);

            if (!result.Any())
            {
                return Ok(result);
            }

            return BadRequest("Error while getting all clients.");
        }

        /// <summary>
        /// Recovers all clients registered in the realm.
        /// </summary>
        /// <returns>
        /// A 200 OK status code along with the list of clients if the operation is successful;
        /// otherwise, a 400 Bad Request status code with an error message, or a 500 Internal Server Error status code if something goes wrong.
        /// </returns>
        /// <param name="addClient">The body related to the client that will be created.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> used to observe cancellation requests for the operation.</param>
        /// <response code="200">The operation was successful, and the list of clients is returned.</response>
        /// <response code="400">The request was invalid or could not be processed.</response>
        /// <response code="500">An internal server error occurred during the processing of the request.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateClient([FromBody] AddClientRequest addClient, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AddClientCommand(addClient), cancellationToken);

            if (result.IsSuccess)
            {
                return Created("/", result.Response);
            }

            return BadRequest("Error while tried created client.");
        }

        [HttpPost("add-client-scope")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiWriter")]
        public async Task<IActionResult> AddClientScope([FromBody] AddClientScopeToClientRequest addClientScopesRequest, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AddClientScopeToClientCommand(addClientScopesRequest), cancellationToken);

            if (result)
            {
                return Created("/", true);
            }

            return BadRequest("Error when tried add client scope to the client. ");
        }
    }
}
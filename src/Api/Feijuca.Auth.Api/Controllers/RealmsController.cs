using Feijuca.Auth.Application.Commands.Realm;
using Feijuca.Auth.Application.Queries.Realm;
using Feijuca.Auth.Application.Requests.Realm;
using Feijuca.Auth.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feijuca.Auth.Api.Controllers
{
    [Route("api/v1/realms")]
    [ApiController]
    [Authorize]
    public class RealmsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Retrieves all configs related to a realm.
        /// </summary>
        /// <returns>
        /// A 201 Created status code containing a realm config if the request is successful;
        /// otherwise, a 400 Bad Request status code with an error message.
        /// </returns>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> that can be used to signal cancellation for the operation.</param>
        /// <param name="name">The name of the realm.</param>
        /// <response code="200">A list of roles associated with the clients in the specified realm.</response>
        /// <response code="400">The request was invalid or could not be processed.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [HttpGet]
        [Route("export/{name}", Name = nameof(ExportRealm))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiReader")]        
        public async Task<IActionResult> ExportRealm([FromRoute] string name, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetRealmConfigurationQuery(name), cancellationToken);

            if (!string.IsNullOrEmpty(name))
            {
                return Created($"/export/{name}", result);
            }

            return BadRequest();
        }

        /// <summary>
        /// Create a new realm on Keycloak
        /// </summary>
        /// <returns>
        /// A 201 Created status code meaning that the realm was created with succesfull.;
        /// otherwise, a 400 Bad Request status code with an error message.
        /// </returns>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> that can be used to signal cancellation for the operation.</param>
        /// <param name="realm">The name of the realm.</param>
        /// <response code="200">A list of roles associated with the clients in the specified realm.</response>
        /// <response code="400">The request was invalid or could not be processed.</response>
        /// <response code="500">An internal server error occurred while processing the request.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddRealm([FromBody] AddRealmRequest realm, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AddRealmsCommand([realm]), cancellationToken);

            if (result.IsSuccess)
            {
                return Created("/api/v1/realm", result);
            }

            return BadRequest();
        }
    }
}

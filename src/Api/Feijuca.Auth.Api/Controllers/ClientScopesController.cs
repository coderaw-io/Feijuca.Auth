using Feijuca.Auth.Application.Commands.ClientScopes;
using Feijuca.Auth.Application.Queries.ClientScopes;
using Feijuca.Auth.Application.Requests.ClientScopes;
using Feijuca.Auth.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Feijuca.Auth.Api.Controllers
{
    [Route("api/v1/clients-scopes")]
    [ApiController]
    [Authorize]
    public class ClientScopesController(IMediator _mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiReader")]        
        public async Task<IActionResult> GetClientScopes(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetClientScopesQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiWriter")]
        public async Task<IActionResult> AddClientScope([FromBody] AddClientScopesRequest addClientScopesRequest, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AddClientScopesCommand([addClientScopesRequest]), cancellationToken);

            if (result.IsSuccess)
            {
                return Ok();
            }

            return BadRequest(result.Error);
        }
    }
}

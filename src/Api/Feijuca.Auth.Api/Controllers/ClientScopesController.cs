using Feijuca.Auth.Application.Commands.ClientScopes;
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
        public IActionResult GetClientScopes()
        {
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiReader")]
        public IActionResult AddClientScope([FromBody] AddClientScopesRequest addClientScopesRequest)
        {
            _mediator.Send(new AddClientScopeCommand(addClientScopesRequest));

            return Ok();
        }
    }
}

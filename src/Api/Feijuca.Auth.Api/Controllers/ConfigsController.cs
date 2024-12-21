using Feijuca.Auth.Application.Commands.Client;
using Feijuca.Auth.Application.Commands.ClientScopes;
using Feijuca.Auth.Application.Commands.Config;
using Feijuca.Auth.Application.Commands.Realm;
using Feijuca.Auth.Application.Queries.Config;
using Feijuca.Auth.Application.Requests.ClientScopes;
using Feijuca.Auth.Application.Requests.Config;
using Feijuca.Auth.Application.Requests.Realm;
using Feijuca.Auth.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Feijuca.Auth.Api.Controllers
{
    [Route("api/v1/config")]
    [ApiController]
    public class ConfigsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Retrieves the existing configuration settings.
        /// </summary>
        /// <returns>
        /// A 200 OK status code along with the configuration if the operation is successful;
        /// otherwise, a 400 Bad Request status code with an error message, or a 500 Internal Server Error status code if something goes wrong.
        /// </returns>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> used to observe cancellation requests for the operation.</param>
        /// <response code="200">The operation was successful, and the configuration settings are returned.</response>
        /// <response code="400">The request was invalid or could not be processed.</response>
        /// <response code="500">An internal server error occurred during the processing of the request.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetConfig(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetConfigQuery(), cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result.Response);
            }

            var responseError = Result<string>.Failure(result.Error);
            return BadRequest(responseError.Error);
        }

        /// <summary>
        /// Inserts a new configuration into the Feijuca.Auth.
        /// </summary>
        /// <returns>
        /// A 201 Created status code along with the newly inserted configuration if the operation is successful;
        /// otherwise, a 400 Bad Request status code with an error message, or a 500 Internal Server Error status code if something goes wrong.
        /// </returns>
        /// <param name="addKeycloakSettings">An object of type <see cref="T:Feijuca.Auth.Common.Models.KeycloakSettings"/> containing the configuration details to be inserted.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken"/> used to observe cancellation requests for the operation.</param>
        /// <response code="201">The configuration was successfully inserted.</response>
        /// <response code="400">The request was invalid or could not be processed.</response>
        /// <response code="500">An internal server error occurred during the processing of the request.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InsertConfig([FromBody] AddKeycloakSettingsRequest addKeycloakSettings, CancellationToken cancellationToken)
        {
            var keyCloakSettings = new KeycloakSettings
            {
                Client = addKeycloakSettings.Client,
                Secrets = addKeycloakSettings.Secrets,
                ServerSettings = addKeycloakSettings.ServerSettings,
                Realms = addKeycloakSettings.Realms
            };

            var realms = new List<AddRealmRequest>();
            foreach (var realm in addKeycloakSettings.Realms)
            {
                realms.Add(new AddRealmRequest(realm.Name, "", realm.DefaultSwaggerTokenGeneration));
            }

            var clientBody = new Application.Requests.Client.AddClientRequest
            {
                ClientId = keyCloakSettings.Client.ClientId,
                Description = "This client is related to Feijuca.Api, this client will handle token generation and keycloak actions.",
                Urls = [$"{Request.Scheme}://{Request.Host}"]
            };

            var clientScopes = new List<AddClientScopesRequest>
            {
                new("feijuca-auth-api", "feijuca-auth-api", true)
            };

            var errorResponse = await ProcessActionsAsync(
                async () => await _mediator.Send(new AddConfigCommand(keyCloakSettings), cancellationToken),
                async () => await _mediator.Send(new AddRealmsCommand(realms), cancellationToken),
                async () => await _mediator.Send(new AddClientCommand(clientBody), cancellationToken),
                async () => await _mediator.Send(new AddClientScopesCommand(clientScopes), cancellationToken)
                );

            if (!errorResponse.IsFailure)
            {
                return BadRequest("Error while tried added Feijuca.Auth.Api client on the Realm.");
            }

            return Created("/api/v1/config", "Initial configs created with succesfully!!!");
        }


        private static async Task<Result> ProcessActionsAsync(params Func<Task<Result<bool>>>[] actions)
        {
            foreach (var action in actions)
            {
                var result = await action();
                if (!result.IsSuccess)
                {
                    return result;
                }
            }

            return Result.Success();
        }
    }
}

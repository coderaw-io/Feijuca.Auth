﻿using Feijuca.Auth.Application.Commands.Role;
using Feijuca.Auth.Application.Queries.Permissions;
using Feijuca.Auth.Application.Requests.Role;
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
    public class RoleController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Get all roles available related to the clients on the realm.
        /// </summary>
        [HttpGet]
        [Route("{tenant}/roles", Name = nameof(GetRoles))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiReader")]
        public async Task<IActionResult> GetRoles([FromRoute] string tenant, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetRolesQuery(tenant), cancellationToken);

            if (result.IsSuccess)
            {
                return Ok(result.Response);
            }

            return BadRequest(Result<string>.Failure(result.Error));
        }

        /// <summary>
        /// Add a new role to a client on the realm.
        /// </summary>
        [HttpPost]
        [Route("{tenant}/role", Name = nameof(AddRole))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiredRole("Feijuca.ApiWriter")]
        public async Task<IActionResult> AddRole([FromRoute] string tenant, [FromBody] AddRoleRequest addRoleRequest, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new AddRoleCommand(tenant, addRoleRequest), cancellationToken);

            if (result.IsSuccess)
            {
                return CreatedAtRoute(nameof(GetRoles), new { tenant }, result.Response); // Retorna 201 Created
            }

            return BadRequest(Result<string>.Failure(result.Error));
        }
    }
}

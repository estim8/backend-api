using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Estim8.Backend.Api.Hubs;
using Estim8.Backend.Api.Hubs.Messages;
using Estim8.Backend.Api.Model;
using Estim8.Backend.Commands.Commands;
using Estim8.Backend.Commands.Services;
using Estim8.Backend.Queries;
using Estim8.Backend.Queries.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace Estim8.Backend.Api.Controllers
{
    /// <summary>
    /// Operations on games
    /// </summary>
    [Route("api/v1/games")]
    [ApiController]
    public class GamesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public GamesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Fetch a game by it's ID
        /// </summary>
        /// <param name="gameId">An active game ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("{gameId}")]
        public async Task<ActionResult<Game>> GetGame(Guid gameId)
        {
            if (!IsInGame(gameId))
                return Unauthorized();
            
            return await _mediator.Send(new GetGameById(gameId));
        }

        /// <summary>
        /// Host a new game
        /// </summary>
        /// <remarks>The game is created in AwaitingPlayers state.
        /// The game's unique ID as well as the unique dealer access token is returned in the body.
        /// Access token must be used for all game management operations (marked with Dealer auth. role)
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("")]
        public async Task<ActionResult<CreateGameResponse>> CreateGame(CreateGameRequest request)
        {
            var id = Guid.NewGuid();
            var playerId = Guid.NewGuid();
            var result = await _mediator.Send(new CreateGame {Id = id, Secret = request.Secret, PlayerId = playerId, Gravatar = request.Gravatar, PlayerName = request.PlayerName});

            if (!result.IsSuccess)
                return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);
            
            return CreatedAtAction(nameof(GetGame), new {gameId = id}, new CreateGameResponse(id)
            {
                PlayerId = playerId,
                Token = new AccessToken(result.Message)             
            });
        }

        /// <summary>
        /// Get game stats for a game
        /// </summary>
        /// <param name="gameId">A game ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("{gameId}/stats")]
        public async Task<IActionResult> GetRoundStats(Guid gameId)
        {
            if (!IsInGame(gameId))
                return Unauthorized();
            
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
using System;
using System.Threading.Tasks;
using Estim8.Backend.Api.Hubs;
using Estim8.Backend.Api.Hubs.Messages;
using Estim8.Backend.Api.Model;
using Estim8.Backend.Commands.Commands;
using Estim8.Backend.Commands.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http2;
using Microsoft.AspNetCore.SignalR;

namespace Estim8.Backend.Api.Controllers
{
    /// <summary>
    /// Operations for players in an active game
    /// </summary>
    [Route("api/v1/games")]
    [ApiController]
    public class PlayersController : ApiControllerBase
    {
        private readonly IHubContext<GameHub, IPlayerClient> _gameHub;
        private readonly IMediator _mediator;

        public PlayersController(IHubContext<GameHub, IPlayerClient> gameHub, IMediator mediator)
        {
            _gameHub = gameHub;
            _mediator = mediator;
        }
        
        /// <summary>
        /// Join an active game
        /// </summary>
        /// <remarks>
        /// Create and add a new player to an active game.
        /// The game secret must be presented.
        /// On successful join, a player access token is returned, which must be used when performing player actions in the game.
        /// </remarks>
        /// <param name="gameId">An active game ID</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{gameId}/players")]
        public async Task<ActionResult<AddPlayerToGameResponse>> AddPlayer(Guid gameId, AddPlayerToGameRequest request)
        {
            var playerId = Guid.NewGuid();

            var result = await _mediator.Send(new AddPlayer
            {
                GameId = gameId, PlayerId = playerId, GameSecret = request.Secret
            });

            if (!result.IsSuccess)
                return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);
            
            await _gameHub.Clients.All.PlayerAddedToGame(new PlayerMessage{GameId = gameId, PlayerId = playerId});

            return Ok(new AddPlayerToGameResponse
            {
                PlayerId = playerId,
                Token = new AccessToken(result.Message)
            });
        }

        /// <summary>
        /// Leave an active game
        /// </summary>
        /// <remarks>
        /// Only the game's dealer or the player himself is allowed to remove the player from the game
        /// </remarks>
        /// <param name="gameId">An active game ID</param>
        /// <param name="playerId">The player ID to remove</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles="Dealer, Player")]
        [Route("{gameId}/players/{playerId}")]
        public async Task<IActionResult> RemovePlayer(Guid gameId, Guid playerId)
        {
            if (!IsInGame(gameId))
                return Unauthorized();

            if (playerId != this.PlayerId && !User.IsInRole("Dealer"))
                return Forbid();

            var result = await _mediator.Send(new RemovePlayer {GameId = gameId, PlayerId = playerId});

            if (!result.IsSuccess)
                return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);
            
            await _gameHub.Clients.All.PlayerRemovedFromGame(new PlayerMessage{GameId = gameId, PlayerId = playerId});
            
            return Ok();
        }
    }
}
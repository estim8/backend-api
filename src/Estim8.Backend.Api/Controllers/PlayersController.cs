using System;
using System.Threading.Tasks;
using Estim8.Backend.Api.Hubs;
using Estim8.Backend.Api.Hubs.Messages;
using Estim8.Backend.Api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Estim8.Backend.Api.Controllers
{
    /// <summary>
    /// Operations for players in an active game
    /// </summary>
    [Route("api/v1/games")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IHubContext<GameHub, IPlayerClient> _gameHub;

        public PlayersController(IHubContext<GameHub, IPlayerClient> gameHub)
        {
            _gameHub = gameHub;
        }
        
        /// <summary>
        /// Add a player to an active game
        /// </summary>
        /// <param name="gameId">An active game ID</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{gameId}/players")]
        public async Task<IActionResult> AddPlayer(Guid gameId, AddPlayerToGameRequest request)
        {
            var playerId = Guid.NewGuid();
            await _gameHub.Clients.All.PlayerAddedToGame(new PlayerMessage{GameId = gameId, PlayerId = playerId});

            return Ok();
        }

        /// <summary>
        /// Remove a player from an active game
        /// </summary>
        /// <param name="gameId">An active game ID</param>
        /// <param name="playerId">The player ID to remove</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{gameId}/players/{playerId}")]
        public async Task<IActionResult> RemovePlayer(Guid gameId, Guid playerId)
        {
            await _gameHub.Clients.All.PlayerRemovedFromGame(new PlayerMessage{GameId = gameId, PlayerId = playerId});
            
            return Ok();
        }
    }
}
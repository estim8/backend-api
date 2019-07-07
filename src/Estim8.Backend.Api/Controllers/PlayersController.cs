using System;
using System.Threading.Tasks;
using Estim8.Backend.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace Estim8.Backend.Api.Controllers
{
    /// <summary>
    /// Operations for players in an active game
    /// </summary>
    [Route("api/games")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
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
            return Ok();
        }
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Estim8.Backend.Api.Controllers
{
    /// <summary>
    /// Play operations in active game
    /// </summary>
    [Route("api/v1/games")]
    [ApiController]
    public class PlayController : ControllerBase
    {
        /// <summary>
        /// Play a card
        /// </summary>
        /// <remarks>
        /// If a card has already been played, it is replaced with the new card
        /// </remarks>
        /// <param name="gameId">An active game ID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{gameId}/rounds/current/playedCard")]
        public async Task<IActionResult> PlayCard(Guid gameId)
        {
            return Ok();
        }

        /// <summary>
        /// Cancel a played card
        /// </summary>
        /// <param name="gameId">An active game ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{gameId}/rounds/current/playedCard")]
        public async Task<IActionResult> RemoveCard(Guid gameId)
        {
            return Ok();
        }
    }
}
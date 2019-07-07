using System;
using System.Threading.Tasks;
using Estim8.Backend.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace Estim8.Backend.Api.Controllers
{
    /// <summary>
    /// Operations for rounds of an active game
    /// </summary>
    [Route("api/games")]
    [ApiController]
    public class RoundsController : ControllerBase
    {
        /// <summary>
        /// Get a round in an active game
        /// </summary>
        /// <param name="gameId">An active game ID</param>
        /// <param name="roundId">A game round in the game</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{gameId:guid}/rounds/{roundId:guid}")]
        public async Task<IActionResult> GetRound(Guid gameId, Guid roundId)
        {
            return Ok();
        }

        /// <summary>
        /// Get round stats for a played round
        /// </summary>
        /// <param name="gameId">An active game ID</param>
        /// <param name="roundId">A game round in the game</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{gameId:guid}/rounds/{roundId:guid}/stats")]
        public async Task<IActionResult> GetRoundStats(Guid gameId, Guid roundId)
        {
            return Ok();
        }

        /// <summary>
        /// Get the current round in an active game
        /// </summary>
        /// <param name="gameId">An active game ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{gameId:guid}/rounds/current")]
        public async Task<IActionResult> GetCurrentRound(Guid gameId)
        {
            return Ok();
        }
    }
}
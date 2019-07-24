using System;
using System.Linq;
using System.Threading.Tasks;
using Estim8.Backend.Api.Model;
using Estim8.Backend.Commands.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Estim8.Backend.Api.Controllers
{
    /// <summary>
    /// Play operations in active game
    /// </summary>
    [Route("api/v1/games")]
    [ApiController]
    public class PlayController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public PlayController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        /// Play a card
        /// </summary>
        /// <remarks>
        /// If a card has already been played, it is replaced with the new card
        /// </remarks>
        /// <param name="gameId">An active game ID</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Player")]
        [Route("{gameId}/rounds/current/playedCard")]
        public async Task<IActionResult> PlayCard(Guid gameId)
        {
            if (!IsInGame(gameId))
                return Unauthorized();
            
            return Ok();
        }

        /// <summary>
        /// Cancel a played card
        /// </summary>
        /// <param name="gameId">An active game ID</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "Player")]
        [Route("{gameId}/rounds/current/playedCard")]
        public async Task<IActionResult> RemoveCard(Guid gameId)
        {
            if (!IsInGame(gameId))
                return Unauthorized();

            return Ok();
        }
        
        /// <summary>
        /// Start a game
        /// </summary>
        /// <remarks>Advances a game from AwaitingPlayers to Playing state</remarks>
        /// <param name="gameId">A game ID</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Dealer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{gameId}/start")]
        public async Task<ActionResult<IdResponse>> StartGame(Guid gameId)
        {
            if (!IsInGame(gameId))
                return Unauthorized();
            
            var result = await _mediator.Send(new StartGame{GameId = gameId, PlayerId = this.PlayerId});

            if (!result.IsSuccess)
                return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);

            return Ok();
        }
    }
}
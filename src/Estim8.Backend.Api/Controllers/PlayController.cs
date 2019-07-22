using System;
using System.Threading.Tasks;
using Estim8.Backend.Api.Model;
using Estim8.Backend.Commands.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
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
        
        /// <summary>
        /// Start a game
        /// </summary>
        /// <remarks>Advances a game from AwaitingPlayers to Playing state</remarks>
        /// <param name="gameId">A game ID</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("{gameId}/start")]
        public async Task<ActionResult<IdResponse>> StartGame(Guid gameId)
        {
            var result = await _mediator.Send(new StartGame{GameId = gameId});

            if (!result.IsSuccess)
                return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);

            return Ok();
        }
    }
}
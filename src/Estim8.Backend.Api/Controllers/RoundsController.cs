using System;
using System.Threading.Tasks;
using Estim8.Backend.Api.Model;
using Estim8.Backend.Commands.Commands;
using Estim8.Backend.Queries.Queries;
using MediatR;
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
        private readonly IMediator _mediator;

        public RoundsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        /// <summary>
        /// Add a new playing round to an active game.
        /// </summary>
        /// <remarks>
        /// This also advances the current round in the game.
        /// Use the versions API to create a new version of an existing round
        /// </remarks>
        /// <param name="gameId">An active game ID</param>
        /// <param name="request">The round to add</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{gameId:guid}/rounds")]
        public async Task<IActionResult> AddRound(Guid gameId, AddGameRoundRequest request)
        {
            var roundId = Guid.NewGuid();
            await _mediator.Send(new AddGameRound
            {
                GameId = gameId,
                Id = roundId,
                RoundVersion = 0,
                Subject = request.Subject
            });

            return CreatedAtAction(nameof(GetRound), new {gameId = gameId, roundId = roundId}, new IdResponse(roundId));
        }
        
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
            return Ok(await _mediator.Send(new GetGameRoundById(gameId, roundId)));
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
            return Ok(await _mediator.Send(new GetCurrentGameRound(gameId)));
        }
    }
}
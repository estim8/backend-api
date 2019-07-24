using System;
using System.Threading.Tasks;
using Estim8.Backend.Api.Model;
using Estim8.Backend.Commands.Commands;
using Estim8.Backend.Queries.Model;
using Estim8.Backend.Queries.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Estim8.Backend.Api.Controllers
{
    /// <summary>
    /// Operations for rounds of an active game
    /// </summary>
    [Route("api/v1/games")]
    [ApiController]
    public class RoundsController : ApiControllerBase
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
        [Authorize(Roles = "Dealer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Route("{gameId}/rounds")]
        public async Task<ActionResult<IdResponse>> AddRound(Guid gameId, AddGameRoundRequest request)
        {
            if (!IsInGame(gameId))
                return Unauthorized();
            
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
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{gameId}/rounds/{roundId}")]
        public async Task<ActionResult<Round>> GetRound(Guid gameId, Guid roundId)
        {
            if (!IsInGame(gameId))
                return Unauthorized();
            
            var round = await _mediator.Send(new GetRoundById(gameId, roundId));

            if (round == null)
                return NotFound();

            return round;
        }

        /// <summary>
        /// Get round stats for a played round
        /// </summary>
        /// <param name="gameId">An active game ID</param>
        /// <param name="roundId">A game round in the game</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("{gameId}/rounds/{roundId}/stats")]
        public async Task<IActionResult> GetRoundStats(Guid gameId, Guid roundId)
        {
            if (!IsInGame(gameId))
                return Unauthorized();
            
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        /// <summary>
        /// Get the current round in an active game
        /// </summary>
        /// <param name="gameId">An active game ID</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("{gameId}/rounds/current")]
        public async Task<ActionResult<Round>> GetCurrentRound(Guid gameId)
        {
            if (!IsInGame(gameId))
                return Unauthorized();
            
            var round = await _mediator.Send(new GetCurrentRound(gameId));

            if (round == null)
                return NotFound();

            return round;
        }
    }
}
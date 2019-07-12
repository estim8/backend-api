using System;
using System.Threading.Tasks;
using Estim8.Backend.Api.Model;
using Estim8.Backend.Commands.Commands;
using Estim8.Backend.Queries;
using Estim8.Backend.Queries.Model;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Estim8.Backend.Api.Controllers
{
    /// <summary>
    /// Operations on games
    /// </summary>
    [Route("api/games")]
    [ApiController]
    public class GamesController : ControllerBase
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("{gameId}")]
        public async Task<ActionResult<Game>> GetGame(Guid gameId)
        {
            return await _mediator.Send(new GetGameById(gameId));
        }

        /// <summary>
        /// Create a new game
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("")]
        public async Task<ActionResult<IdResponse>> CreateGame(CreateGameRequest request)
        {
            var id = Guid.NewGuid();
            var result = await _mediator.Send(new CreateGame {Id = id, Secret = request.Secret, CardsetId = request.CardSetId});

            if (!result.IsSuccess)
                return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);

            return CreatedAtAction(nameof(GetGame), new {gameId = id}, new IdResponse(id));
        }
        
        /// <summary>
        /// Get game stats for a game
        /// </summary>
        /// <param name="gameId">A game ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{gameId}/stats")]
        public async Task<IActionResult> GetRoundStats(Guid gameId)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }

        /// <summary>
        /// End a game
        /// </summary>
        /// <remarks>
        /// Closes the game for new rounds
        /// </remarks>
        /// <param name="gameId">An active game ID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{gameId}/end")]
        public async Task<IActionResult> EndGame(Guid gameId)
        {
            return StatusCode(StatusCodes.Status501NotImplemented);
        }
    }
}
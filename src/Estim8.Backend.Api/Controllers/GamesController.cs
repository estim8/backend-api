using System;
using System.Threading.Tasks;
using Estim8.Backend.Api.Model;
using Estim8.Backend.Commands.Commands;
using Estim8.Backend.Queries;
using Estim8.Backend.Queries.Model;
using MediatR;
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
        [Route("{gameId}")]
        public async Task<Game> GetGame(Guid gameId)
        {
            return await _mediator.Send(new GetGameById(gameId));
        }

        /// <summary>
        /// Create a new game
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateGame(CreateGameRequest request)
        {
            var id = Guid.NewGuid();
            var result = await _mediator.Send(new CreateGame {Id = id, Secret = request.Secret, CardsetId = request.CardSetId});

            if (!result.IsSuccess)
                return StatusCode(500, result.ErrorMessage);

            return CreatedAtAction(nameof(GetGame), new {gameId = id}, new IdResponse(id));
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
            return Ok();
        }
    }
}
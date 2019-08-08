using System;
using System.Linq;
using System.Threading.Tasks;
using Estim8.Backend.Api.Hubs;
using Estim8.Backend.Api.Hubs.Messages;
using Estim8.Backend.Api.Model;
using Estim8.Backend.Commands.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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
        private readonly IHubContext<GameHub, IPlayerClient> _gameHub;

        public PlayController(IMediator mediator, IHubContext<GameHub, IPlayerClient> gameHub)
        {
            _mediator = mediator;
            _gameHub = gameHub;
        }
        
        /// <summary>
        /// Play a card
        /// </summary>
        /// <remarks>
        /// If a card has already been played, it is replaced with the new card
        /// </remarks>
        /// <param name="gameId">An active game ID</param>
        /// <param name="request">The card played</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Player")]
        [Route("{gameId}/rounds/current/playedCard")]
        public async Task<IActionResult> PlayCard(Guid gameId, PlayCardRequest request)
        {
            if (!IsInGame(gameId))
                return Unauthorized();

            var response = await _mediator.Send(new PlayCard
                {GameId = gameId, PlayerId = this.PlayerId, Type = request.CardType, Value = request.Value});

            if (!response.IsSuccess)
                return StatusCode(StatusCodes.Status500InternalServerError, response.ErrorMessage);

            await _gameHub.Clients.Group(gameId.ToString()).CardPlayed(new PlayedCardMessage
            {
                Action = PlayedCardMessage.CardAction.Play,
                CardType = request.CardType,
                CardValue = request.Value,
                PlayerId = this.PlayerId
            });
            
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

            var response = await _mediator.Send(new CancelCard
                {GameId = gameId, PlayerId = this.PlayerId});

            if (!response.IsSuccess)
                return StatusCode(StatusCodes.Status500InternalServerError, response.ErrorMessage);

            await _gameHub.Clients.Group(gameId.ToString()).CardPlayed(new PlayedCardMessage
            {
                Action = PlayedCardMessage.CardAction.Cancel,
                PlayerId = this.PlayerId
            });
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
            
            await _gameHub.Clients.Group(gameId.ToString()).GameStarted(new GameMessage {GameId = gameId});

            return Ok();
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
        [Authorize(Roles="Dealer")]
        [Route("{gameId}/end")]
        public async Task<IActionResult> EndGame(Guid gameId)
        {
            if (!IsInGame(gameId))
                return Unauthorized();

            var result = await _mediator.Send(new EndGame {GameId = gameId});

            if (!result.IsSuccess)
                return StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage);

            await _gameHub.Clients.Group(gameId.ToString()).GameEnded(new GameMessage {GameId = gameId});
            
            return Ok();
        } 
    }
}
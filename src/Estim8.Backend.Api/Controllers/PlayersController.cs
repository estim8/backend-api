using System;
using System.Threading.Tasks;
using Estim8.Backend.Api.Hubs;
using Estim8.Backend.Api.Hubs.Messages;
using Estim8.Backend.Api.Model;
using Estim8.Backend.Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Estim8.Backend.Api.Controllers
{
    /// <summary>
    /// Operations for players in an active game
    /// </summary>
    [Route("api/v1/games")]
    [ApiController]
    public class PlayersController : ApiControllerBase
    {
        private readonly IHubContext<GameHub, IPlayerClient> _gameHub;
        private readonly ISecurityTokenService _securityTokenService;

        public PlayersController(IHubContext<GameHub, IPlayerClient> gameHub, ISecurityTokenService securityTokenService)
        {
            _gameHub = gameHub;
            _securityTokenService = securityTokenService;
        }
        
        /// <summary>
        /// Add a player to an active game
        /// </summary>
        /// <param name="gameId">An active game ID</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{gameId}/players")]
        public async Task<ActionResult<AddPlayerToGameResponse>> AddPlayer(Guid gameId, AddPlayerToGameRequest request)
        {
            var playerId = Guid.NewGuid();
            await _gameHub.Clients.All.PlayerAddedToGame(new PlayerMessage{GameId = gameId, PlayerId = playerId});

            var token = _securityTokenService.IssueToken(gameId, playerId, new[] {PlayerRoles.Player.ToString()});
            
            return Ok(new AddPlayerToGameResponse
            {
                PlayerId = playerId,
                Token = new AccessToken
                {
                    Access_Token = token,
                    Token_Type = "Bearer",
                    Expires_In = 3600
                }
            });
        }

        /// <summary>
        /// Remove a player from an active game
        /// </summary>
        /// <param name="gameId">An active game ID</param>
        /// <param name="playerId">The player ID to remove</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles="Dealer, Player")]
        [Route("{gameId}/players/{playerId}")]
        public async Task<IActionResult> RemovePlayer(Guid gameId, Guid playerId)
        {
            if (!IsInGame(gameId))
                return Unauthorized();

            if (playerId != this.PlayerId && !User.IsInRole("Dealer"))
                return Forbid();
            
            await _gameHub.Clients.All.PlayerRemovedFromGame(new PlayerMessage{GameId = gameId, PlayerId = playerId});
            
            return Ok();
        }
    }
}
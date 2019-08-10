using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Estim8.Backend.Api.Hubs.Messages;
using Estim8.Backend.Persistence.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Estim8.Backend.Api.Hubs
{
    [Authorize]
    public class GameHub : Hub<IPlayerClient>
    {
        private readonly IPlayerRepository _playerRepo;

        public GameHub(IPlayerRepository playerRepo)
        {
            _playerRepo = playerRepo;
        }
        public override async Task OnConnectedAsync()
        {
            var gameId = Context.User.FindFirstValue("game_id");
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (Guid.TryParse(Context.User.FindFirstValue("game_id"), out var gameId) &&
                Guid.TryParse(Context.User.FindFirstValue("player_id"), out var playerId))
            {
                await _playerRepo.DeletePlayer(gameId, playerId);
                
                await Clients.OthersInGroup(gameId.ToString())
                    .PlayerRemovedFromGame(new PlayerMessage {GameId = gameId, PlayerId = playerId});
            }
            
            await base.OnDisconnectedAsync(exception);
        }
        
    }
}
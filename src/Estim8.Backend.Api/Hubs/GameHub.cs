using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Estim8.Backend.Api.Hubs
{
    [Authorize]
    public class GameHub : Hub<IPlayerClient>
    {
        public override Task OnConnectedAsync()
        {
            var gameId = Context.User.FindFirstValue("game_id");
            Groups.AddToGroupAsync(Context.ConnectionId, gameId);
            return base.OnConnectedAsync();
        }
    }
}
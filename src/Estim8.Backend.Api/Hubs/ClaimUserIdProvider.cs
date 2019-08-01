using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Estim8.Backend.Api.Hubs
{
    public class ClaimUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirstValue("player_id");
        }
    }
}
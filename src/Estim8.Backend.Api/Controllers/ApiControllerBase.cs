using System;
using Microsoft.AspNetCore.Mvc;

namespace Estim8.Backend.Api.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected Guid PlayerId => Guid.Parse(HttpContext.User.FindFirst("player_id")?.Value ?? "");
        
        protected bool IsInGame(Guid gameId)
        {
            return HttpContext.User.HasClaim("game_id", gameId.ToString());
        }
    }
}
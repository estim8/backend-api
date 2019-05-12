using System;
using System.Threading.Tasks;
using Estim8.Backend.Api.Model;
using Microsoft.AspNetCore.Mvc;

namespace Estim8.Backend.Api.Controllers
{
    public partial class GamesController
    {
        [HttpPost]
        [Route("{id}/players")]
        public async Task<IActionResult> AddPlayer(Guid id, AddPlayerToGameRequest request)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("{id}/players/{playerId}")]
        public async Task<IActionResult> RemovePlayer(Guid id, Guid playerId)
        {
            return Ok();
        }
    }
}
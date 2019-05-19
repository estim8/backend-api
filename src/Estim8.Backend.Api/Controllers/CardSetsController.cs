using System;
using System.Collections;
using System.Threading.Tasks;
using Estim8.Backend.Queries.Model;
using Microsoft.AspNetCore.Mvc;

namespace Estim8.Backend.Api.Controllers
{
    [Route("api/[controller]")]
    public class CardSetsController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetCardSets()
        {
            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCard(Guid id)
        {
            return Ok();
        }
        
    }
}
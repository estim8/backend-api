using System;
using System.Collections;
using System.Threading.Tasks;
using Estim8.Backend.Queries.Model;
using Microsoft.AspNetCore.Mvc;

namespace Estim8.Backend.Api.Controllers
{
    /// <summary>
    /// Types of cards (cardsets) used in games
    /// </summary>
    [Route("api/v1/cardsets")]
    public class CardsetsController : ControllerBase
    {
        /// <summary>
        /// Get all available cardsets
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetCardSets()
        {
            return Ok();
        }

        /// <summary>
        /// Get a specific cardset
        /// </summary>
        /// <param name="setId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{setId}")]
        public async Task<IActionResult> GetCardSet(Guid setId)
        {
            return Ok();
        }
        
    }
}
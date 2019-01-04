using System;
using System.Threading.Tasks;
using Estim8.Backend.Queries;
using Estim8.Backend.Queries.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Estim8.Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GamesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<Game> GetGame(Guid id)
        {
            return await _mediator.Send(new GetGameById(id));
        }

        [HttpPost]
        [Route("")]
        public async Task CreateGame()
        {
            
        }
    }
}
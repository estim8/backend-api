using System;
using System.Net.Http;
using System.Threading.Tasks;
using Estim8.Backend.Api.Model;
using Estim8.Backend.Commands.Commands;
using Estim8.Backend.Queries;
using Estim8.Backend.Queries.Model;
using LamarCompiler.Scenarios;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Estim8.Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class GamesController : ControllerBase
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
        public async Task<IActionResult> CreateGame()
        {
            var id = Guid.NewGuid();
            var result = await _mediator.Send(new CreateGame {Id = id});

            if (!result.IsSuccess)
                return StatusCode(500, result.ErrorMessage);

            return CreatedAtAction(nameof(GetGame), new {id = id}, new IdResponse(id));
        }
    }
}
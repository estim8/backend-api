using System;
using System.Security.Claims;
using MediatR;

namespace Estim8.Backend.Commands.Commands
{
    public class StartGame : IRequest<Response>
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
    }
}
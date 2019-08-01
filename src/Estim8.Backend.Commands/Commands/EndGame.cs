using System;
using MediatR;

namespace Estim8.Backend.Commands.Commands
{
    public class EndGame : IRequest<Response>
    {
        public Guid GameId { get; set; }
    }
}
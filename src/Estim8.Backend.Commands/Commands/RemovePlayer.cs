using System;
using MediatR;

namespace Estim8.Backend.Commands.Commands
{
    public class RemovePlayer : IRequest<Response>
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
    }
}
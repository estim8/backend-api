using System;
using MediatR;

namespace Estim8.Backend.Commands.Commands
{
    public class CancelCard : IRequest<Response>
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
    }
}
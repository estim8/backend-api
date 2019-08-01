using System;
using MediatR;

namespace Estim8.Backend.Commands.Commands
{
    public class PlayCard : IRequest<Response>
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
        public string Type { get; set; }
        public double? Value { get; set; }
    }
}
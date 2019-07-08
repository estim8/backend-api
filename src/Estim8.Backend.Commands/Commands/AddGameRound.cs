using System;
using MediatR;

namespace Estim8.Backend.Commands.Commands
{
    public class AddGameRound : IRequest<Response>
    {
        public Guid GameId { get; set; }
        public Guid Id { get; set; }
        public int RoundVersion { get; set; }
        public string Subject { get; set; }
    }
}
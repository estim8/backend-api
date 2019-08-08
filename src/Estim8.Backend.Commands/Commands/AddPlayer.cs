using System;
using Estim8.Backend.Commands.Services;
using MediatR;

namespace Estim8.Backend.Commands.Commands
{
    public class AddPlayer : IRequest<Response<SerializedSecurityToken>>
    {
        public Guid PlayerId { get; set; }
        public Guid GameId { get; set; }
        public string GameSecret { get; set; }
        public string PlayerName { get; set; }
        public string Gravatar { get; set; }
    }
}
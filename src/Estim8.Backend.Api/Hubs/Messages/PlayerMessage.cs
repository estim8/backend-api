using System;

namespace Estim8.Backend.Api.Hubs.Messages
{
    public class PlayerMessage
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string Gravatar { get; set; }
    }
}
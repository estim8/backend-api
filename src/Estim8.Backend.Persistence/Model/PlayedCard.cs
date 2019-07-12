using System;

namespace Estim8.Backend.Persistence.Model
{
    public class PlayedCard : Entity
    {
        public Guid GameId { get; set; }
        public Guid GameRoundId { get; set; }
        public Guid PlayerId { get; set; }
        public int CardValue { get; set; }
        public DateTimeOffset PlayedTimestamp { get; set; }
    }
}
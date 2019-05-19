using System;
using System.Collections;
using System.Collections.Generic;

namespace Estim8.Backend.Persistence.Model
{
    public class GameRound : Entity
    {
        public int RoundNumber { get; set; }
        public int RoundVersion { get; set; }
        public DateTimeOffset StartedTimestamp { get; set; }
        public DateTimeOffset EndedTimestamp { get; set; }
        public string Subject { get; set; }
        public IEnumerable<Entity> PlayedCards { get; set; }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;

namespace Estim8.Backend.Persistence.Model
{
    public class GameRound
    {
        public int RoundVersion { get; set; }
        public DateTimeOffset StartedTimestamp { get; set; }
        public DateTimeOffset EndedTimestamp { get; set; }
        public string Subject { get; set; }
        public IEnumerable<PlayedCard> PlayedCards { get; set; }
    }
}
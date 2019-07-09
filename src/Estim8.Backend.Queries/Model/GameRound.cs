using System;

namespace Estim8.Backend.Queries.Model
{
    public class GameRound
    {
        public Guid Id { get; set; }
        public int RoundVersion { get; set; }
        public DateTimeOffset StartedTimestamp { get; set; }
        public DateTimeOffset EndedTimestamp { get; set; }
        public string Subject { get; set; }
    }
}
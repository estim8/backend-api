using System;

namespace Estim8.Backend.Queries.Model
{
    public class Round
    {
        public Guid Id { get; set; }
        public int RoundVersion { get; set; }
        public DateTimeOffset StartedTimestamp { get; set; }
        public DateTimeOffset? EndedTimestamp { get; set; }
        public string Subject { get; set; }
        public Guid GameId { get; set; }
    }
}
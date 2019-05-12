using System;

namespace Estim8.Backend.Persistence.Model
{
    public class PlayedCard
    {
        public Guid UserId { get; set; }
        
        public double Value { get; set; }
        public DateTimeOffset PlayedTimestamp { get; set; }
    }
}
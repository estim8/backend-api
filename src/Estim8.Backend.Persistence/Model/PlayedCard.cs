using System;

namespace Estim8.Backend.Persistence.Model
{
    public class PlayedCard
    {
        public Guid UserId { get; set; }
        
        /// <summary>
        /// Card scale value: [scale-id]/[scale-value]
        /// </summary>
        public string Value { get; set; }
        public DateTimeOffset PlayedTimestamp { get; set; }
    }
}
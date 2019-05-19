using System;

namespace Estim8.Backend.Persistence.Model
{
    public class PlayedCard : Entity
    {
        public Guid PlayerId { get; set; }
        public Guid CardSetId { get; set; }
        public DateTimeOffset PlayedTimestamp { get; set; }
    }
}
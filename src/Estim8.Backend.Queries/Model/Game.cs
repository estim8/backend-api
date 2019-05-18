using System;

namespace Estim8.Backend.Queries.Model
{
    public class Game : IModel
    {
        public Guid Id { get; set; }
        public string PublicId { get; set; }
        public DateTimeOffset CreatedTimestamp { get; set; }
    }
}
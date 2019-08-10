using System;
using System.Collections.Generic;

namespace Estim8.Backend.Queries.Model
{
    public class Game : IModel
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedTimestamp { get; set; }
        public Guid DealerId { get; set; }
        public IEnumerable<Player> Players { get; set; }
        public GameState State { get; set; }
    }
    
    public enum GameState
    {
        AwaitingPlayers,
        Playing,
        Ended
    }
}
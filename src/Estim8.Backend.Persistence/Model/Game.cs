using System;
using System.Collections;
using System.Collections.Generic;

namespace Estim8.Backend.Persistence.Model
{
    public class Game : Entity
    {
        // Some reference on datamodelling on CosmosDB: https://docs.microsoft.com/en-us/azure/cosmos-db/modeling-data
        
        public string PublicId { get; set; }
        public Guid PointScaleType { get; set; }
        public DateTimeOffset CreatedTimestamp { get; set; }
        public DateTimeOffset StartedTimestamp { get; set; }
        public DateTimeOffset EndedTimestamp { get; set; }

        public IEnumerable<Player> Players { get; set; }

        public IEnumerable<GameRound> Rounds { get; set; }
        public Game(Guid id)
        {
            Id = id;
        }
    }
}
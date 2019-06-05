using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;

namespace Estim8.Backend.Persistence.Model
{
    [ProtoContract]
    public class Game : Entity
    {
        [ProtoMember(1)]
        public string PublicId { get; set; }
        [ProtoMember(2)]
        public Guid CardSetId { get; set; }
        [ProtoMember(3)]
        public DateTimeOffset CreatedTimestamp { get; set; }
        [ProtoMember(4)]
        public DateTimeOffset StartedTimestamp { get; set; }
        [ProtoMember(5)]
        public DateTimeOffset EndedTimestamp { get; set; }

        [ProtoMember(6)]
        public IEnumerable<Entity> Players { get; set; }

        [ProtoMember(7)]
        public IEnumerable<Entity> Rounds { get; set; }
        
        [ProtoMember(8)] public string Secret { get; set; }
        
        public Game(Guid id)
        {
            Id = id;
        }

        public Game()
        {
            
        }
    }
}
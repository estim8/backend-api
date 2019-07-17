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
        [ProtoMember(8)]
        public string Secret { get; set; }
        [ProtoMember(9)]
        public GameState State { get; set; }
    }

    public enum GameState
    {
        AwaitingPlayers,
        Playing,
        Ended
    }
}
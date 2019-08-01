using System;
using ProtoBuf;

namespace Estim8.Backend.Persistence.Model
{
    [ProtoContract]
    public class PlayedCard
    {
        [ProtoMember(1)] public Guid GameId { get; set; }
        [ProtoMember(2)] public Guid GameRoundId { get; set; }
        [ProtoMember(3)] public Guid PlayerId { get; set; }
        [ProtoMember(4)] public string CardType { get; set; }
        [ProtoMember(5)] public double CardValue { get; set; }
        [ProtoMember(6)] public DateTimeOffset PlayedTimestamp { get; set; }
    }
}
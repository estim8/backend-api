using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;

namespace Estim8.Backend.Persistence.Model
{
    [ProtoContract]
    public class Round : Entity
    {
        [ProtoMember(1)] public int RoundVersion { get; set; }
        [ProtoMember(2)] public DateTimeOffset StartedTimestamp { get; set; }
        [ProtoMember(3)] public DateTimeOffset? EndedTimestamp { get; set; }
        [ProtoMember(4)] public string Subject { get; set; }
    }
}
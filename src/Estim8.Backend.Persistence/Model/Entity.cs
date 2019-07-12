using System;
using ProtoBuf;

namespace Estim8.Backend.Persistence.Model
{
    [ProtoContract]
    [ProtoInclude(10, typeof(Game))]
    [ProtoInclude(11, typeof(Round))]
    public abstract class Entity
    {
        [ProtoMember(1)]
        public Guid Id { get; set; }
    }
}
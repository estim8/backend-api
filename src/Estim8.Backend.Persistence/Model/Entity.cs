using System;
using ProtoBuf;

namespace Estim8.Backend.Persistence.Model
{
    public class Entity
    {
        [ProtoMember(0)]
        public Guid Id { get; protected set; }
    }
}
using CachingFramework.Redis.Contracts;
using StackExchange.Redis.Extensions.Protobuf;

namespace Estim8.Backend.Persistence.ProtoBuf
{
    public class ProtoBufSerializer : ProtobufSerializer, ISerializer
    {
        public byte[] Serialize<T>(T value)
        {
            return (this as ProtobufSerializer).Serialize(value);
        }
    }
}
using System;
using ProtoBuf.Meta;

namespace Estim8.Backend.Persistence.ProtoBuf
{
    public class ProtoBufConfig
    {
        public static void Configure()
        {
            RuntimeTypeModel.Default.Add(typeof(DateTimeOffset), false).SetSurrogate(typeof(DateTimeOffsetSurrogate));
        }
    }
}
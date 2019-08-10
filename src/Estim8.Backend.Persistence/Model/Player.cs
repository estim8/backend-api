using ProtoBuf;

namespace Estim8.Backend.Persistence.Model
{
    [ProtoContract]
    public class Player : Entity
    {
        [ProtoMember(1)] public string Email { get; set; }
        [ProtoMember(2)] public string PlayerName { get; set; }
    }
}
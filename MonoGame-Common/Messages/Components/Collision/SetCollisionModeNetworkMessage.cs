using LiteNetLib.Utils;
using MonoGame_Common.Attributes;
using MonoGame_Common.Enums;

namespace MonoGame_Common.Messages.Components.Collision;

[NetworkMessage(17)]
public class SetCollisionModeNetworkMessage : NetworkMessage
{
    public SetCollisionModeNetworkMessage()
    {
    }

    public SetCollisionModeNetworkMessage(string uuid, CollisionMode mode)
    {
        UUID = uuid;
        Mode = mode;
    }

    public string UUID { get; set; }
    public CollisionMode Mode { get; set; }

    public override void Deserialize(NetDataReader reader)
    {
        UUID = reader.GetString();
        Mode = (CollisionMode)reader.GetByte();
    }

    public override NetDataWriter Serialize()
    {
        var data = new NetDataWriter();
        data.Put(GetNetworkTypeId());
        data.Put(UUID);
        data.Put((byte)Mode);
        return data;
    }
}
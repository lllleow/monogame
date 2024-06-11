using LiteNetLib.Utils;
using MonoGame_Common.Attributes;
using System.Numerics;

namespace MonoGame_Common.Messages.Player;

[NetworkMessage(5)]
public class SpawnPlayerNetworkMessage : NetworkMessage
{
    public SpawnPlayerNetworkMessage()
    {
    }

    public SpawnPlayerNetworkMessage(string uuid, Vector2 position)
    {
        UUID = uuid;
        Position = position;
    }

    public string UUID { get; set; }
    public Vector2 Position { get; set; }

    public override void Deserialize(NetDataReader reader)
    {
        UUID = reader.GetString();
        Position = new Vector2(reader.GetFloat(), reader.GetFloat());
    }

    public override NetDataWriter Serialize()
    {
        var data = new NetDataWriter();
        data.Put(GetNetworkTypeId());
        data.Put(UUID);
        data.Put(Position.X);
        data.Put(Position.Y);
        return data;
    }
}
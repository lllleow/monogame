using LiteNetLib.Utils;
using MonoGame_Common.Attributes;
using MonoGame_Common.Enums;

namespace MonoGame_Common.Messages.Player;

[NetworkMessage(15)]
public class KeyClickedNetworkMessage : NetworkMessage
{
    public KeyClickedNetworkMessage()
    {
    }

    public KeyClickedNetworkMessage(string uuid, List<Keys> keys)
    {
        UUID = uuid;
        Keys = keys;
    }

    public string UUID { get; set; }
    public List<Keys> Keys { get; set; } = [];

    public override void Deserialize(NetDataReader reader)
    {
        UUID = reader.GetString();
        var count = reader.GetInt();
        for (var i = 0; i < count; i++) Keys.Add((Keys)reader.GetInt());
    }

    public override NetDataWriter Serialize()
    {
        var data = new NetDataWriter();
        data.Put(GetNetworkTypeId());
        data.Put(UUID);
        data.Put(Keys.Count);
        foreach (var key in Keys) data.Put((int)key);

        return data;
    }
}
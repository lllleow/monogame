using System;
using LiteNetLib.Utils;

namespace MonoGame;

public class HandshakeMessage : INetworkMessage
{
    public string UUID { get; set; }

    public HandshakeMessage()
    {
    }

    public NetDataWriter Serialize()
    {
        NetDataWriter data = new NetDataWriter();
        data.Put((byte)GetMessageTypeId());
        data.Put(UUID);

        return data;
    }

    public NetDataReader Deserialize(NetDataReader reader)
    {
        UUID = reader.GetString();
        return reader;
    }

    public int GetMessageTypeId()
    {
        return 0;
    }
}

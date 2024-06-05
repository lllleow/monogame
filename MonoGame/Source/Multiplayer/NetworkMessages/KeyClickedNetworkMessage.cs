using System;
using LiteNetLib.Utils;
using MonoGame.Source.Multiplayer.Interfaces;

namespace MonoGame;

public class KeyClickedNetworkMessage : INetworkMessage
{
    public int Key;

    public NetDataReader Deserialize(NetDataReader reader)
    {
        Key = reader.GetInt();
        return reader;
    }

    public int GetMessageTypeId()
    {
        return (int)NetworkMessageTypes.KeyClickedEvent;
    }

    public NetDataWriter Serialize()
    {
        NetDataWriter writer = new NetDataWriter();
        writer.Put(Key);
        return writer;
    }
}

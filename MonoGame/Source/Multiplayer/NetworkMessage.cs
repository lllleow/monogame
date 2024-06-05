using System;
using LiteNetLib.Utils;
using MonoGame.Source.Multiplayer.Interfaces;

namespace MonoGame;

public class NetworkMessage : INetworkMessage
{
    public NetworkMessage()
    {
    }

    public NetworkMessage(NetDataReader reader)
    {
        Deserialize(reader);
    }

    public virtual NetDataReader Deserialize(NetDataReader reader)
    {
        throw new NotImplementedException();
    }

    public virtual int GetMessageTypeId()
    {
        throw new NotImplementedException();
    }

    public virtual NetDataWriter Serialize()
    {
        throw new NotImplementedException();
    }
}

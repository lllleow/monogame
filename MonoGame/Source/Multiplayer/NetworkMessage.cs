using System;
using LiteNetLib.Utils;

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

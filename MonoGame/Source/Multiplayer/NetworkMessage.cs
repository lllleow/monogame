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

    public virtual void Deserialize(NetDataReader reader)
    {
        throw new NotImplementedException();
    }

    public void Execute()
    {
        throw new NotImplementedException();
    }

    public virtual NetDataWriter Serialize()
    {
        throw new NotImplementedException();
    }

    public bool Validate()
    {
        throw new NotImplementedException();
    }
}

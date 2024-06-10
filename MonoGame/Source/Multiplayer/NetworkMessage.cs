using System;
using LiteNetLib.Utils;
using MonoGame_Common.Messages;
using MonoGame_Common.Util;
using MonoGame.Source.Multiplayer.Interfaces;

namespace MonoGame.Source.Multiplayer;

public class NetworkMessage : INetworkMessage
{
    protected int MessageTypeId { get; set; }

    public NetworkMessage()
    {
        MessageTypeId = MessageRegistry.Instance.GetIdByType(GetType());
    }

    public NetworkMessage(NetDataReader reader)
    {
        Deserialize(reader);
    }

    public virtual void Deserialize(NetDataReader reader)
    {
        throw new NotImplementedException();
    }

    public virtual NetDataWriter Serialize()
    {
        throw new NotImplementedException();
    }

    protected byte GetNetworkTypeId()
    {
        return (byte)MessageTypeId;
    }
}

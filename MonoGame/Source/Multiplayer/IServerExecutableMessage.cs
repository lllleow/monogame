using System;
using LiteNetLib;
using LiteNetLib.Utils;

namespace MonoGame;

public interface IServerExecutableMessage
{
    public void ExecuteOnServer(NetPeer peer, NetDataReader reader, DeliveryMethod deliveryMethod, byte channel);
}

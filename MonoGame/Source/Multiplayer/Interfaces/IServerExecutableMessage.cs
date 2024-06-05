using System;
using LiteNetLib;
using LiteNetLib.Utils;

namespace MonoGame.Source.Multiplayer.Interfaces;

public interface IServerExecutableMessage
{
    public void ExecuteOnServer(NetPeer peer, NetDataReader reader, DeliveryMethod deliveryMethod, byte channel);
}

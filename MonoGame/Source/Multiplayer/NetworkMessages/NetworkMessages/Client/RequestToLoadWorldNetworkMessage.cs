using System;
using System.Numerics;
using LiteNetLib.Utils;

namespace MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Client
{
    public class RequestToLoadWorldNetworkMessage : NetworkMessage
    {
        public RequestToLoadWorldNetworkMessage()
        {

        }

        public override void Deserialize(NetDataReader reader)
        {

        }

        public override NetDataWriter Serialize()
        {
            NetDataWriter data = new NetDataWriter();
            data.Put((byte)NetworkMessageTypes.RequestToLoadWorldNetworkMessage);
            return data;
        }
    }
}
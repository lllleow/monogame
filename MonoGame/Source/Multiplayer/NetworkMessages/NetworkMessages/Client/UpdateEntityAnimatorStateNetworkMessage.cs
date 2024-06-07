using System;
using LiteNetLib.Utils;
using MonoGame.Source.Multiplayer;

namespace MonoGame
{
    public class UpdateEntityAnimatorStateNetworkMessage : NetworkMessage
    {
        public UpdateEntityAnimatorStateNetworkMessage() : base()
        {
        }

        public override void Deserialize(NetDataReader reader)
        {
        }

        public override NetDataWriter Serialize()
        {
            NetDataWriter data = new NetDataWriter();
            data.Put((byte)GetNetworkTypeId());
            return data;
        }
    }
}
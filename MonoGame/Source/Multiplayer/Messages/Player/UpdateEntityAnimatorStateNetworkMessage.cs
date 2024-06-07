using System;
using LiteNetLib.Utils;

namespace MonoGame.Source.Multiplayer.Messages.Player
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
            var data = new NetDataWriter();
            data.Put(GetNetworkTypeId());
            return data;
        }
    }
}
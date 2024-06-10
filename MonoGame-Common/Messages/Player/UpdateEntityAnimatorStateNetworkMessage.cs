using System;
using LiteNetLib.Utils;
using MonoGame_Common.Messages;

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
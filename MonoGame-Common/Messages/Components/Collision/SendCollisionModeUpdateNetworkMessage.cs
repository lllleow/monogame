using System;
using LiteNetLib.Utils;
using MonoGame;
using MonoGame_Common.Messages;

namespace MonoGame
{

    [NetworkMessage(14)]
    public class SendCollisionModeUpdateNetworkMessage : NetworkMessage
    {
        public string UUID { get; set; }
        public CollisionMode Mode { get; set; }

        public SendCollisionModeUpdateNetworkMessage()
        {
        }

        public SendCollisionModeUpdateNetworkMessage(string uuid, CollisionMode mode)
        {
            this.UUID = uuid;
            this.Mode = mode;
        }

        public override void Deserialize(NetDataReader reader)
        {
            this.UUID = reader.GetString();
            this.Mode = (CollisionMode)reader.GetByte();
        }

        public override NetDataWriter Serialize()
        {
            NetDataWriter data = new NetDataWriter();
            data.Put(GetNetworkTypeId());
            data.Put(this.UUID);
            data.Put((byte)this.Mode);
            return data;
        }
    }
}
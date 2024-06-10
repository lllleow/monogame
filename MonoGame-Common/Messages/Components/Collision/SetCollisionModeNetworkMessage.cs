using System;
using LiteNetLib.Utils;
using MonoGame.Source.Systems.Components.Collision.Enum;
using MonoGame_Common.Messages;

namespace MonoGame
{
    [NetworkMessage(17)]
    public class SetCollisionModeNetworkMessage : NetworkMessage
    {
        public string UUID { get; set; }
        public CollisionMode Mode { get; set; }

        public SetCollisionModeNetworkMessage()
        {
        }

        public SetCollisionModeNetworkMessage(string uuid, CollisionMode mode)
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
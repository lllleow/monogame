using LiteNetLib.Utils;
using MonoGame_Common.Attributes;
using MonoGame_Common.Enums;

namespace MonoGame_Common.Messages.Components.Collision
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
            UUID = uuid;
            Mode = mode;
        }

        public override void Deserialize(NetDataReader reader)
        {
            UUID = reader.GetString();
            Mode = (CollisionMode)reader.GetByte();
        }

        public override NetDataWriter Serialize()
        {
            var data = new NetDataWriter();
            data.Put(GetNetworkTypeId());
            data.Put(UUID);
            data.Put((byte)Mode);
            return data;
        }
    }
}
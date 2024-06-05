using System;
using System.Numerics;
using LiteNetLib.Utils;

namespace MonoGame
{
    public class UpdatePlayerPositionNetworkMessage : NetworkMessage
    {
        public string UUID;
        public Vector2 Position;

        public UpdatePlayerPositionNetworkMessage()
        {

        }

        public UpdatePlayerPositionNetworkMessage(string uuid, Vector2 position)
        {
            Position = position;
            UUID = uuid;
        }

        public override void Deserialize(NetDataReader reader)
        {
            UUID = reader.GetString();
            Position = new Vector2(reader.GetFloat(), reader.GetFloat());
        }

        public override NetDataWriter Serialize()
        {
            NetDataWriter data = new NetDataWriter();
            data.Put((byte)NetworkMessageTypes.UpdatePlayerPositionNetworkMessage);
            data.Put(UUID);
            data.Put(Position.X);
            data.Put(Position.Y);
            return data;
        }
    }
}
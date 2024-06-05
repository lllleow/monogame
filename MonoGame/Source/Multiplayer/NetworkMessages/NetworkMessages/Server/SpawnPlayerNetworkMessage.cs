using System;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;

namespace MonoGame
{
    public class SpawnPlayerNetworkMessage : NetworkMessage
    {
        public string UUID;
        public Vector2 Position;

        public SpawnPlayerNetworkMessage()
        {

        }

        public SpawnPlayerNetworkMessage(string uuid, Vector2 position)
        {
            UUID = uuid;
            Position = position;
        }

        public override void Deserialize(NetDataReader reader)
        {
            UUID = reader.GetString();
            Position = new Vector2(reader.GetFloat(), reader.GetFloat());
        }

        public override NetDataWriter Serialize()
        {
            NetDataWriter data = new NetDataWriter();
            data.Put((byte)NetworkMessageTypes.SpawnPlayerNetworkMessage);
            data.Put(UUID);
            data.Put(Position.X);
            data.Put(Position.Y);
            return data;
        }
    }
}
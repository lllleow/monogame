﻿using LiteNetLib.Utils;
using Microsoft.Xna.Framework;

namespace MonoGame
{
    public class MovePlayerNetworkMessage : NetworkMessage
    {
        public string UUID { get; set; }
        public Vector2 Speed { get; set; }
        public Direction Direction { get; set; }
        public Vector2 ExpectedPosition { get; set; }

        public MovePlayerNetworkMessage()
        {
        }

        public MovePlayerNetworkMessage(string uuid, Vector2 speed, Direction direction, Vector2 expectedPosition)
        {
            UUID = uuid;
            Speed = speed;
            Direction = direction;
            ExpectedPosition = expectedPosition;
        }

        public override void Deserialize(NetDataReader reader)
        {
            UUID = reader.GetString();
            Speed = new Vector2(reader.GetFloat(), reader.GetFloat());
            Direction = (Direction)reader.GetByte();
            ExpectedPosition = new Vector2(reader.GetFloat(), reader.GetFloat());
        }

        public override NetDataWriter Serialize()
        {
            NetDataWriter data = new NetDataWriter();
            data.Put((byte)NetworkMessageTypes.MovePlayerNetworkMessage);
            data.Put(UUID);
            data.Put(Speed.X);
            data.Put(Speed.Y);
            data.Put((byte)Direction);
            data.Put(ExpectedPosition.X);
            data.Put(ExpectedPosition.Y);
            return data;
        }
    }
}
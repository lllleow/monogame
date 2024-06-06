using LiteNetLib.Utils;
using Microsoft.Xna.Framework;

namespace MonoGame
{
    public class RequestMovementNetworkMessage : NetworkMessage
    {
        public Vector2 Speed { get; set; }
        public Direction Direction { get; set; }

        public RequestMovementNetworkMessage()
        {
        }

        public RequestMovementNetworkMessage(Vector2 displacement, Direction direction)
        {
            Speed = displacement;
            Direction = direction;
        }

        public override void Deserialize(NetDataReader reader)
        {
            Speed = new Vector2(reader.GetFloat(), reader.GetFloat());
            Direction = (Direction)reader.GetByte();
        }

        public override NetDataWriter Serialize()
        {
            NetDataWriter data = new NetDataWriter();
            data.Put((byte)NetworkMessageTypes.RequestMovementNetworkMessage);
            data.Put(Speed.X);
            data.Put(Speed.Y);
            data.Put((byte)Direction);
            return data;
        }
    }
}
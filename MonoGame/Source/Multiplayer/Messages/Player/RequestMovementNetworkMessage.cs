using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using MonoGame.Source.Util.Enum;

namespace MonoGame.Source.Multiplayer.Messages.Player
{
    [NetworkMessage(6)]
    public class RequestMovementNetworkMessage : NetworkMessage
    {
        public Vector2 Speed { get; set; }
        public Direction Direction { get; set; }

        public RequestMovementNetworkMessage() : base()
        {
        }

        public RequestMovementNetworkMessage(Vector2 displacement, Direction direction) : base()
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
            var data = new NetDataWriter();
            data.Put(GetNetworkTypeId());
            data.Put(Speed.X);
            data.Put(Speed.Y);
            data.Put((byte)Direction);
            return data;
        }
    }
}
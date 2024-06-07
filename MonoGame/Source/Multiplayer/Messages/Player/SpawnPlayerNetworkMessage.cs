using LiteNetLib.Utils;
using Microsoft.Xna.Framework;

namespace MonoGame.Source.Multiplayer.Messages.Player
{
    [NetworkMessage(5)]
    public class SpawnPlayerNetworkMessage : NetworkMessage
    {
        public string UUID { get; set; }
        public Vector2 Position { get; set; }

        public SpawnPlayerNetworkMessage() : base()
        {
        }

        public SpawnPlayerNetworkMessage(string uuid, Vector2 position) : base()
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
            var data = new NetDataWriter();
            data.Put(GetNetworkTypeId());
            data.Put(UUID);
            data.Put(Position.X);
            data.Put(Position.Y);
            return data;
        }
    }
}
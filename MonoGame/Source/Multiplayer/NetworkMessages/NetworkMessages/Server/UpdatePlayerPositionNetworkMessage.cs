using System.Numerics;
using LiteNetLib.Utils;

namespace MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Server
{
    [NetworkMessage(7)]
    public class UpdatePlayerPositionNetworkMessage : NetworkMessage
    {
        public string UUID { get; set; }
        public Vector2 Position { get; set; }

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
            var data = new NetDataWriter();
            data.Put(GetNetworkTypeId());
            data.Put(UUID);
            data.Put(Position.X);
            data.Put(Position.Y);
            return data;
        }
    }
}
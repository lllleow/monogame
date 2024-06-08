using LiteNetLib.Utils;
using MonoGame.Source.Multiplayer;

namespace MonoGame
{
    [NetworkMessage(12)]
    public class SendAnimatorStateNetworkMessage : NetworkMessage
    {
        public string UUID { get; set; }
        public int CurrentTime { get; set; }
        public int CurrentTextureX { get; set; }
        public int CurrentTextureY { get; set; }

        public SendAnimatorStateNetworkMessage(string uuid, int currentTime, int currentTextureX, int currentTextureY)
        {
            UUID = uuid;
            CurrentTime = currentTime;
            CurrentTextureX = currentTextureX;
            CurrentTextureY = currentTextureY;
        }

        public override void Deserialize(NetDataReader reader)
        {
            UUID = reader.GetString();
            CurrentTime = reader.GetInt();
            CurrentTextureX = reader.GetInt();
            CurrentTextureY = reader.GetInt();
        }

        public override NetDataWriter Serialize()
        {
            NetDataWriter data = new NetDataWriter();
            data.Put((byte)GetNetworkTypeId());
            data.Put(UUID);
            data.Put(CurrentTime);
            data.Put(CurrentTextureX);
            data.Put(CurrentTextureY);
            return data;
        }
    }
}
using System;
using LiteNetLib.Utils;
using MonoGame.Source.Multiplayer;

namespace MonoGame
{
    [NetworkMessage(13)]
    public class UpdateAnimatorStateNetworkMessage : NetworkMessage
    {
        public string UUID { get; set; }
        public int CurrentTime { get; set; }
        public int TextureX { get; set; }
        public int TextureY { get; set; }

        public UpdateAnimatorStateNetworkMessage()
        {
        }

        public UpdateAnimatorStateNetworkMessage(string uuid, int currentTime, int textureX, int textureY)
        {
            UUID = uuid;
            CurrentTime = currentTime;
            TextureX = textureX;
            TextureY = textureY;
        }

        public override void Deserialize(NetDataReader reader)
        {
            UUID = reader.GetString();
            CurrentTime = reader.GetInt();
            TextureX = reader.GetInt();
            TextureY = reader.GetInt();
        }

        public override NetDataWriter Serialize()
        {
            NetDataWriter data = new NetDataWriter();
            data.Put(GetNetworkTypeId());
            data.Put(UUID);
            data.Put(CurrentTime);
            data.Put(TextureX);
            data.Put(TextureY);
            return data;
        }
    }
}
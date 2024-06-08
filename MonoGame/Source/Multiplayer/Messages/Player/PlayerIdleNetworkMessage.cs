using System;
using LiteNetLib.Utils;
using MonoGame;
using MonoGame.Source.Multiplayer;

namespace MonoGame
{
    [NetworkMessage(14)]
    public class PlayerIdleNetworkMessage : NetworkMessage
    {
        public string UUID { get; set; }

        public PlayerIdleNetworkMessage()
        {
        }

        public PlayerIdleNetworkMessage(string uuid)
        {
            UUID = uuid;
        }

        public override void Deserialize(NetDataReader reader)
        {
            UUID = reader.GetString();
        }

        public override NetDataWriter Serialize()
        {
            NetDataWriter data = new NetDataWriter();
            data.Put(GetNetworkTypeId());
            data.Put(UUID);
            return data;
        }
    }
}
using System;
using LiteNetLib.Utils;
using MonoGame_Common;
using MonoGame_Common.Attributes;
using MonoGame_Common.Messages;

namespace MonoGame
{
    [NetworkMessage(19)]
    public class ChangeGameModeNetworkMessage : NetworkMessage
    {
        required public string UUID { get; set; }
        required public GameMode DesiredGameMode { get; set; }

        public ChangeGameModeNetworkMessage()
        {
        }

        public override void Deserialize(NetDataReader reader)
        {
            UUID = reader.GetString();
            DesiredGameMode = (GameMode)reader.GetByte();
        }

        public override NetDataWriter Serialize()
        {
            NetDataWriter data = new NetDataWriter();
            data.Put(GetNetworkTypeId());
            data.Put(UUID);
            data.Put((byte)DesiredGameMode);
            return data;
        }
    }
}
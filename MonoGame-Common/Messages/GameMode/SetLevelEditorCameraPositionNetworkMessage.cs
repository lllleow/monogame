using System;
using System.Numerics;
using LiteNetLib.Utils;
using MonoGame_Common.Attributes;
using MonoGame_Common.Messages;

namespace MonoGame
{
    [NetworkMessage(21)]
    public class SetLevelEditorCameraPositionNetworkMessage : NetworkMessage
    {
        required public string UUID { get; set; }
        required public Vector2 Position { get; set; }

        public SetLevelEditorCameraPositionNetworkMessage()
        {
        }

        public override void Deserialize(NetDataReader reader)
        {
            UUID = reader.GetString();
            Position = new Vector2(reader.GetFloat(), reader.GetFloat());
        }

        public override NetDataWriter Serialize()
        {
            NetDataWriter data = new NetDataWriter();
            data.Put(GetNetworkTypeId());
            data.Put(UUID);
            data.Put(Position.X);
            data.Put(Position.Y);
            return data;
        }
    }
}
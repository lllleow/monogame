using System;
using System.Collections.Generic;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework.Input;
using MonoGame_Common.Messages;

namespace MonoGame
{
    [NetworkMessage(15)]
    public class KeyClickedNetworkMessage : NetworkMessage
    {
        public string UUID { get; set; }
        public List<Keys> Keys { get; set; } = new();

        public KeyClickedNetworkMessage()
        {
        }

        public KeyClickedNetworkMessage(string uuid, List<Keys> keys)
        {
            UUID = uuid;
            Keys = keys;
        }

        public override void Deserialize(NetDataReader reader)
        {
            UUID = reader.GetString();
            int count = reader.GetInt();
            for (int i = 0; i < count; i++)
            {
                Keys.Add((Keys)reader.GetInt());
            }
        }

        public override NetDataWriter Serialize()
        {
            NetDataWriter data = new NetDataWriter();
            data.Put(GetNetworkTypeId());
            data.Put(UUID);
            data.Put(Keys.Count);
            foreach (Keys key in Keys)
            {
                data.Put((int)key);
            }

            return data;
        }
    }
}
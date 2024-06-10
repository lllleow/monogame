using System;
using LiteNetLib.Utils;
using MonoGame_Common.Messages;

namespace MonoGame
{
    [NetworkMessage(16)]
    public class RegisterEntityComponentNetworkMessage : NetworkMessage
    {
        public string UUID { get; set; }
        public Type ComponentType { get; set; }

        public RegisterEntityComponentNetworkMessage()
        {
        }

        public RegisterEntityComponentNetworkMessage(string uuid, Type componentType)
        {
            UUID = uuid;
            ComponentType = componentType;
        }

        public override void Deserialize(NetDataReader reader)
        {
            UUID = reader.GetString();
            string typeIdentifier = reader.GetString();
            ComponentType = Type.GetType(typeIdentifier);
        }

        public override NetDataWriter Serialize()
        {
            NetDataWriter data = new NetDataWriter();
            data.Put(GetNetworkTypeId());
            data.Put(UUID);
            string typeName = ComponentType.FullName;
            data.Put(typeName);
            return data;
        }
    }
}
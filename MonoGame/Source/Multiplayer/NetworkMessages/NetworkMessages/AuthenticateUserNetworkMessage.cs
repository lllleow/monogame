using System;
using LiteNetLib.Utils;

namespace MonoGame;

public class AuthenticateUserNetworkMessage : NetworkMessage
{
    public string UUID { get; set; }

    public AuthenticateUserNetworkMessage()
    {
    }

    public AuthenticateUserNetworkMessage(string uuid)
    {
        UUID = uuid;
    }

    public override NetDataWriter Serialize()
    {
        NetDataWriter data = new NetDataWriter();
        data.Put((byte)NetworkMessageTypes.AuthenticateUserNetworkMessage);
        data.Put(UUID);

        return data;
    }

    public override NetDataReader Deserialize(NetDataReader reader)
    {
        UUID = reader.GetString();
        return reader;
    }
}

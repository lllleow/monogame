using LiteNetLib.Utils;
using MonoGame_Common.Attributes;

namespace MonoGame_Common.Messages.Authentication;

[NetworkMessage(1)]
public class AuthenticateUserNetworkMessage : NetworkMessage
{
    public string UUID { get; set; }

    public AuthenticateUserNetworkMessage() : base()
    {
    }

    public AuthenticateUserNetworkMessage(string uuid) : base()
    {
        UUID = uuid;
    }

    public override NetDataWriter Serialize()
    {
        var data = new NetDataWriter();
        data.Put(GetNetworkTypeId());
        data.Put(UUID);

        return data;
    }

    public override void Deserialize(NetDataReader reader)
    {
        UUID = reader.GetString();
    }
}

using LiteNetLib.Utils;
using MonoGame_Common.Attributes;

namespace MonoGame_Common.Messages.Authentication;

[NetworkMessage(2)]
public class AuthenticationResultNetworkMessage : NetworkMessage
{
    public AuthenticationResultNetworkMessage()
    {
    }

    public AuthenticationResultNetworkMessage(bool success, string reason)
    {
        Success = success;
        Reason = reason;
    }

    public bool Success { get; set; }
    public string Reason { get; set; }

    public override void Deserialize(NetDataReader reader)
    {
        Success = reader.GetBool();
        Reason = reader.GetString();
    }

    public override NetDataWriter Serialize()
    {
        var data = new NetDataWriter();
        data.Put(GetNetworkTypeId());
        data.Put(Success);
        data.Put(Reason);

        return data;
    }
}
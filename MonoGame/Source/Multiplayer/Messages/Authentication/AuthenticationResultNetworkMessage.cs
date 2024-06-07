using LiteNetLib.Utils;

namespace MonoGame.Source.Multiplayer.Messages.Authentication;

[NetworkMessage(2)]
public class AuthenticationResultNetworkMessage : NetworkMessage
{
    public bool Success { get; set; }
    public string Reason { get; set; }

    public AuthenticationResultNetworkMessage() : base()
    {
    }

    public AuthenticationResultNetworkMessage(bool success, string reason) : base()
    {
        Success = success;
        Reason = reason;
    }

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

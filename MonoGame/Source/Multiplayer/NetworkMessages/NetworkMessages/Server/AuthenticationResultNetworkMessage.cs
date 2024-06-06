using LiteNetLib.Utils;

namespace MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Server;

public class AuthenticationResultNetworkMessage : NetworkMessage
{
    public bool Success { get; set; }
    public string Reason { get; set; }

    public AuthenticationResultNetworkMessage()
    {
    }

    public AuthenticationResultNetworkMessage(bool success, string reason)
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
        data.Put((byte)NetworkMessageTypes.AuthenticationResultNetworkMessage);
        data.Put(Success);
        data.Put(Reason);

        return data;
    }
}

using LiteNetLib.Utils;
using MonoGame_Common.Util;

namespace MonoGame_Common.Messages;

public class NetworkMessage : INetworkMessage
{
    protected int MessageTypeId { get; set; }

    public NetworkMessage()
    {
        MessageTypeId = MessageRegistry.Instance.GetIdByType(GetType());
    }

    public NetworkMessage(NetDataReader reader)
    {
        Deserialize(reader);
    }

    public virtual void Deserialize(NetDataReader reader)
    {
        throw new NotImplementedException();
    }

    public virtual NetDataWriter Serialize()
    {
        throw new NotImplementedException();
    }

    protected byte GetNetworkTypeId()
    {
        return (byte)MessageTypeId;
    }
}

using LiteNetLib.Utils;

namespace MonoGame_Common.Messages;

public interface INetworkMessage
{
    public NetDataWriter Serialize();
    public void Deserialize(NetDataReader reader);
}

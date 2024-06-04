using LiteNetLib.Utils;

namespace MonoGame;

public interface INetworkMessage
{
    public NetDataWriter Serialize();
    public NetDataReader Deserialize(NetDataReader reader);
    public int GetMessageTypeId();
}

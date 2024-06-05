using LiteNetLib.Utils;

namespace MonoGame.Source.Multiplayer.Interfaces;

public interface INetworkMessage
{
    public NetDataWriter Serialize();
    public NetDataReader Deserialize(NetDataReader reader);
}

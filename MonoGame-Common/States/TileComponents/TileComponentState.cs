using LiteNetLib.Utils;

namespace MonoGame_Common;

public class TileComponentState : INetSerializable
{
    public virtual void Deserialize(NetDataReader reader)
    {
        throw new NotImplementedException();
    }

    public virtual void Serialize(NetDataWriter writer)
    {
        throw new NotImplementedException();
    }
}

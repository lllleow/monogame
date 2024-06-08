using LiteNetLib.Utils;

namespace MonoGame.Source.States.Components;

public abstract class ComponentState : INetSerializable
{
    public virtual void Deserialize(NetDataReader reader)
    {
        throw new System.NotImplementedException();
    }

    public virtual void Serialize(NetDataWriter writer)
    {
        throw new System.NotImplementedException();
    }
}

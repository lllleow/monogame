using System;
using LiteNetLib.Utils;

namespace MonoGame.Source.States.Components;

public class ComponentState : INetSerializable
{
    // public Type ComponentStateType { get; set; }

    public ComponentState()
    {
        // ComponentStateType = GetType();
    }

    public virtual void Deserialize(NetDataReader reader)
    {
        // string componentStateType = reader.GetString();
        // ComponentStateType = Type.GetType(componentStateType);
    }

    public virtual void Serialize(NetDataWriter writer)
    {
        // writer.Put(ComponentStateType.FullName);
    }
}

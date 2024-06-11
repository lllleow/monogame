using LiteNetLib.Utils;
using MonoGame_Common.States.Components;
using System.Numerics;

namespace MonoGame_Common.States;

public class EntityState : INetSerializable
{
    public required string UUID { get; set; }
    public Vector2 Position { get; set; }
    public List<ComponentState> Components { get; set; } = [];

    public virtual void Serialize(NetDataWriter writer)
    {
        writer.Put(UUID);
        writer.Put(Position.X);
        writer.Put(Position.Y);
    }

    public virtual void Deserialize(NetDataReader reader)
    {
        UUID = reader.GetString();
        Position = new Vector2(reader.GetFloat(), reader.GetFloat());
    }

    // public EntityState(IGameEntity entity)
    // {
    //     UUID = entity.UUID;
    //     Position = entity.Position;
    // }
    public T GetComponent<T>()
        where T : ComponentState
    {
        return (T)Components.Find(x => x.GetType() == typeof(T));
    }

    public T ReplaceComponent<T>(T component)
        where T : ComponentState
    {
        _ = Components.RemoveAll(x => x.GetType() == component.GetType());
        Components.Add(component);
        return component;
    }

    public T AddComponent<T>(T component)
        where T : ComponentState
    {
        Components.Add(component);
        return component;
    }

    public bool HasComponent(Type t)
    {
        return Components.Exists(x => x.GetType() == t);
    }
}
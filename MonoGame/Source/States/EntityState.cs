using System.Collections.Generic;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using MonoGame.Source.States.Components;
using MonoGame.Source.Systems.Entity.Interfaces;

namespace MonoGame.Source.States;

public class EntityState : INetSerializable
{
    public Vector2 Position { get; set; }
    public List<ComponentState> Components { get; set; } = new();

    public EntityState()
    {
    }

    public EntityState(IGameEntity entity)
    {
        Position = entity.Position;
    }

    public T GetComponent<T>()
    where T : ComponentState
    {
        return (T)Components.Find(x => x.GetType() == typeof(T));
    }

    public T SetComponent<T>(T component)
    where T : ComponentState
    {
        Components.Add(component);
        return component;
    }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(Position.X);
        writer.Put(Position.Y);
    }

    public void Deserialize(NetDataReader reader)
    {
        Position = new Vector2(reader.GetFloat(), reader.GetFloat());
    }
}

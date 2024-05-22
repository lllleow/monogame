using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoGame;

public abstract class GameEntity : IGameEntity
{
    public List<IEntityComponent> components { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Speed { get; set; }

    public GameEntity()
    {
        components = new List<IEntityComponent>();
        Position = Vector2.Zero;
        Speed = Vector2.Zero;
    }

    public virtual void Update(GameTime gameTime)
    {
        foreach (var component in components)
        {
            component.Update(gameTime);
        }
    }

    public void AddComponent(IEntityComponent component)
    {
        components.Add(component);
        component.Initialize();
    }

    public void RemoveComponent(IEntityComponent component)
    {
        components.Remove(component);
    }

    public T GetFirstComponent<T>()
    {
        return GetComponents<T>().FirstOrDefault();
    }

    public List<T> GetComponents<T>()
    {
        return components.OfType<T>().ToList();
    }

    public bool ContainsComponent<T>()
    {
        return components.OfType<T>().Any();
    }
}

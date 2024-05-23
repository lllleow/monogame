using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (var component in components)
        {
            component.Draw(spriteBatch);
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

    public Vector2 GetDisplacement(Direction direction, Vector2 speed)
    {
        switch (direction)
        {
            case Direction.TOP:
                return new Vector2(0, -speed.Y);
            case Direction.BOTTOM:
                return new Vector2(0, speed.Y);
            case Direction.LEFT:
                return new Vector2(-speed.X, 0);
            case Direction.RIGHT:
                return new Vector2(speed.X, 0);
            case Direction.LEFT_TOP:
                return new Vector2(-speed.X, -speed.Y);
            case Direction.RIGHT_TOP:
                return new Vector2(speed.X, -speed.Y);
            case Direction.LEFT_BOTTOM:
                return new Vector2(-speed.X, speed.Y);
            case Direction.RIGHT_BOTTOM:
                return new Vector2(speed.X, speed.Y);
            default:
                return Vector2.Zero;
        }
    }

    public bool Move(Direction direction, Vector2 speed)
    {
        Vector2 displacement = GetDisplacement(direction, speed);
        Vector2 newPosition = Position + displacement;

        if (IsValidPosition(newPosition))
        {
            Position = newPosition;
            return true;
        }

        return false;
    }

    public bool IsValidPosition(Vector2 position)
    {
        return true;
    }
}

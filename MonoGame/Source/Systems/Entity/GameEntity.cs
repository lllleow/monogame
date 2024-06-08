using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Components.Animator;
using MonoGame.Source.Systems.Components.Interfaces;
using MonoGame.Source.Systems.Entity.Interfaces;
using MonoGame.Source.Systems.Tiles;
using MonoGame.Source.Util.Enum;

namespace MonoGame.Source.Systems.Entity;

public abstract class GameEntity : IGameEntity
{
    public List<IEntityComponent> Components { get; set; }

    public Vector2 Position { get; set; }

    public string UUID { get; set; } = Guid.NewGuid().ToString();

    public GameEntity()
    {
        Components = [];
        Position = Vector2.Zero;
    }

    public virtual void Update(GameTime gameTime)
    {
        foreach (var component in Components)
        {
            component.Update(gameTime);
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (var component in Components)
        {
            component.Draw(spriteBatch);
        }
    }

    public void AddComponent(IEntityComponent component)
    {
        Components.Add(component);
        component.SetEntity(this);
        component.Initialize();
        component.Initialized = true;
    }

    public void RemoveComponent(IEntityComponent component)
    {
        _ = Components.Remove(component);
    }

    public T GetFirstComponent<T>()
    {
        return GetComponents<T>().FirstOrDefault();
    }

    public List<T> GetComponents<T>()
    {
        return Components.OfType<T>().ToList();
    }

    public bool ContainsComponent<T>()
    {
        return Components.OfType<T>().Any();
    }

    public void Teleport(Vector2 newPosition)
    {
        Position = newPosition;
    }

    public Rectangle GetEntityBoundsAtPosition(Vector2 position)
    {
        if (ContainsComponent<AnimatorComponent>())
        {
            _ = GetFirstComponent<AnimatorComponent>();
            return new Rectangle((int)position.X, (int)position.Y, Tile.PixelSizeX, Tile.PixelSizeY);
        }
        else
        {
            return Rectangle.Empty;
        }
    }
}

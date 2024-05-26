using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Components.Animator;
using MonoGame.Source.Systems.Components.Collision;
using MonoGame.Source.Systems.Components.Interfaces;
using MonoGame.Source.Systems.Entity.Interfaces;

namespace MonoGame.Source.Systems.Entity;

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
            case Direction.Up:
                return new Vector2(0, -speed.Y);
            case Direction.Down:
                return new Vector2(0, speed.Y);
            case Direction.Left:
                return new Vector2(-speed.X, 0);
            case Direction.Right:
                return new Vector2(speed.X, 0);
            case Direction.LeftUp:
                return new Vector2(-speed.X, -speed.Y);
            case Direction.RightUp:
                return new Vector2(speed.X, -speed.Y);
            case Direction.LeftDown:
                return new Vector2(-speed.X, speed.Y);
            case Direction.RightDown:
                return new Vector2(speed.X, speed.Y);
            default:
                return Vector2.Zero;
        }
    }

    public bool Move(GameTime gameTime, Direction direction, Vector2 speed)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 displacement = GetDisplacement(direction, speed) * deltaTime * 1000;
        Vector2 newPosition = Vector2.Lerp(Position, Position + displacement, deltaTime);

        if (CanMove(newPosition, direction))
        {
            Position = newPosition;
            return true;
        }

        return false;
    }

    public void Teleport(Vector2 newPosition)
    {
        Position = newPosition;
    }


    public bool CanMove(Vector2 newPosition, Direction direction)
    {
        AnimatorComponent animator = GetFirstComponent<AnimatorComponent>();
        Rectangle entityRectangle = new Rectangle((int)newPosition.X, (int)newPosition.Y, Tile.PixelSizeX, Tile.PixelSizeY);
        bool[,] mask = CollisionMaskHandler.GetMaskForTexture(animator.AnimationBundle.SpriteSheet, animator.GetSpriteRectangle());

        List<ITile> tiles = Globals.world.GetTilesIntersecting(mask, entityRectangle);
        return tiles.Count == 0;
    }
}

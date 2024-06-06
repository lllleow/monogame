using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Components.Animator;
using MonoGame.Source.Systems.Components.Collision;
using MonoGame.Source.Systems.Components.Collision.Enum;
using MonoGame.Source.Systems.Components.Interfaces;
using MonoGame.Source.Systems.Components.PixelBounds;
using MonoGame.Source.Systems.Entity.Interfaces;
using MonoGame.Source.Systems.Entity.PlayerNamespace;
using MonoGame.Source.Systems.Tiles;
using MonoGame.Source.Systems.Tiles.Interfaces;
using MonoGame.Source.Util.Enum;

namespace MonoGame.Source.Systems.Entity;

public abstract class GameEntity : IGameEntity
{
    public List<IEntityComponent> Components { get; set; }

    public Vector2 Position { get; set; }

    public Vector2 Speed { get; set; }

    public string UUID { get; set; } = Guid.NewGuid().ToString();

    public GameEntity()
    {
        Components = [];
        Position = Vector2.Zero;
        Speed = Vector2.Zero;
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

    public Vector2 GetDisplacement(Direction direction, Vector2 speed)
    {
        return direction switch
        {
            Direction.Up => new Vector2(0, -speed.Y),
            Direction.Down => new Vector2(0, speed.Y),
            Direction.Left => new Vector2(-speed.X, 0),
            Direction.Right => new Vector2(speed.X, 0),
            Direction.LeftUp => new Vector2(-speed.X, -speed.Y),
            Direction.RightUp => new Vector2(speed.X, -speed.Y),
            Direction.LeftDown => new Vector2(-speed.X, speed.Y),
            Direction.RightDown => new Vector2(speed.X, speed.Y),
            _ => Vector2.Zero,
        };
    }

    public void Move(GameTime gameTime, Direction direction, Vector2 speed)
    {
        if (this is Player)
        {
            Vector2 displacement = GetDisplacement(direction, speed);
            Vector2 newPosition = Position + displacement;

            if (ContainsComponent<AnimatorComponent>())
            {
                AnimatorComponent animator = GetFirstComponent<AnimatorComponent>();

                switch (direction)
                {
                    case Direction.Up:
                        animator.PlayAnimation("walking_back");
                        break;
                    case Direction.Down:
                        animator.PlayAnimation("walking_front");
                        break;
                    case Direction.Left:
                        animator.PlayAnimation("walking_left");
                        break;
                    case Direction.Right:
                        animator.PlayAnimation("walking_right");
                        break;
                    default:
                        animator.PlayAnimation("idle");
                        break;
                }
            }

            Position = newPosition;
        }
    }

    public void Teleport(Vector2 newPosition)
    {
        Position = newPosition;
    }

    public bool CanMove(Vector2 newPosition, Direction direction)
    {
        if (ContainsComponent<CollisionComponent>())
        {
            Rectangle entityRectangle = GetEntityBoundsAtPosition(newPosition);
            CollisionComponent collisionComponent = GetFirstComponent<CollisionComponent>();
            List<ITile> tiles;

            if (collisionComponent.Mode == CollisionMode.BoundingBox)
            {
                tiles = collisionComponent.GetTilesCollidingWithRectangle(entityRectangle);
            }
            else if (collisionComponent.Mode is CollisionMode.PixelPerfect or CollisionMode.CollisionMask)
            {
                PixelBoundsComponent pixelBounds = GetFirstComponent<PixelBoundsComponent>();
                tiles = collisionComponent.GetTilesCollidingWithMask(pixelBounds.Mask, entityRectangle);
            }
            else
            {
                return true;
            }

            return tiles.Count == 0;
        }

        return true;
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

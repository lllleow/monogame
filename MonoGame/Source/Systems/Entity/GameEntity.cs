using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Components.Animator;
using MonoGame.Source.Systems.Components.Collision;
using MonoGame.Source.Systems.Components.Interfaces;

namespace MonoGame.Source.Systems.Entity;

public abstract class GameEntity : IGameEntity
{

    public List<IEntityComponent> components { get; set; }

    public Vector2 Position { get; set; }

    public Vector2 Speed { get; set; }

    public string UUID { get; set; } = Guid.NewGuid().ToString();

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
        component.SetEntity(this);
        component.Initialize();
        component.Initialized = true;
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
            else if (collisionComponent.Mode == CollisionMode.PixelPerfect || collisionComponent.Mode == CollisionMode.CollisionMask)
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
            AnimatorComponent animator = GetFirstComponent<AnimatorComponent>();
            return new Rectangle((int)position.X, (int)position.Y, Tile.PixelSizeX, Tile.PixelSizeY);
        }
        else
        {
            return Rectangle.Empty;
        }
    }
}

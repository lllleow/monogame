using System;
using System.Collections.Generic;
using System.Linq;
using LiteNetLib;
using LiteNetLib.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Components.Animator;
using MonoGame.Source.Systems.Components.Collision;
using MonoGame.Source.Systems.Components.Interfaces;
using MonoGame.Source.Systems.Entity.Interfaces;

namespace MonoGame.Source.Systems.Entity;

/// <summary>
/// Represents a game entity in the game world.
/// </summary>
public abstract class GameEntity : IGameEntity
{
    /// <summary>
    /// Gets or sets the list of components attached to the entity.
    /// </summary>
    public List<IEntityComponent> components { get; set; }

    /// <summary>
    /// Gets or sets the position of the entity.
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// Gets or sets the speed of the entity.
    /// </summary>
    public Vector2 Speed { get; set; }

    public string UUID { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Initializes a new instance of the <see cref="GameEntity"/> class.
    /// </summary>
    public GameEntity()
    {
        components = new List<IEntityComponent>();
        Position = Vector2.Zero;
        Speed = Vector2.Zero;
    }

    /// <summary>
    /// Updates the entity's components.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    public virtual void Update(GameTime gameTime)
    {
        foreach (var component in components)
        {
            component.Update(gameTime);
        }
    }

    /// <summary>
    /// Draws the entity's components.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used for drawing.</param>
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (var component in components)
        {
            component.Draw(spriteBatch);
        }
    }

    /// <summary>
    /// Adds a component to the entity.
    /// </summary>
    /// <param name="component">The component to add.</param>
    public void AddComponent(IEntityComponent component)
    {
        components.Add(component);
        component.SetEntity(this);
        component.Initialize();
        component.Initialized = true;
    }

    /// <summary>
    /// Removes a component from the entity.
    /// </summary>
    /// <param name="component">The component to remove.</param>
    public void RemoveComponent(IEntityComponent component)
    {
        components.Remove(component);
    }

    /// <summary>
    /// Gets the first component of the specified type attached to the entity.
    /// </summary>
    /// <typeparam name="T">The type of the component.</typeparam>
    /// <returns>The first component of the specified type, or null if not found.</returns>
    public T GetFirstComponent<T>()
    {
        return GetComponents<T>().FirstOrDefault();
    }

    /// <summary>
    /// Gets all components of the specified type attached to the entity.
    /// </summary>
    /// <typeparam name="T">The type of the components.</typeparam>
    /// <returns>A list of components of the specified type.</returns>
    public List<T> GetComponents<T>()
    {
        return components.OfType<T>().ToList();
    }

    /// <summary>
    /// Checks if the entity contains a component of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the component.</typeparam>
    /// <returns>True if the entity contains a component of the specified type, otherwise false.</returns>
    public bool ContainsComponent<T>()
    {
        return components.OfType<T>().Any();
    }

    /// <summary>
    /// Gets the displacement vector for the specified direction and speed.
    /// </summary>
    /// <param name="direction">The direction of movement.</param>
    /// <param name="speed">The speed of movement.</param>
    /// <returns>The displacement vector.</returns>
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

    /// <summary>
    /// Moves the entity in the specified direction with the specified speed.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    /// <param name="direction">The direction of movement.</param>
    /// <param name="speed">The speed of movement.</param>
    /// <returns>True if the entity successfully moved, otherwise false.</returns>
    public void Move(GameTime gameTime, Direction direction, Vector2 speed)
    {
        if (this is Player)
        {
            Vector2 displacement = GetDisplacement(direction, speed);
            Vector2 newPosition = Position + displacement;
            Position = newPosition;
        }
    }

    /// <summary>
    /// Teleports the entity to the specified position.
    /// </summary>
    /// <param name="newPosition">The new position of the entity.</param>
    public void Teleport(Vector2 newPosition)
    {
        Position = newPosition;
    }

    /// <summary>
    /// Checks if the entity can move to the specified position in the specified direction.
    /// </summary>
    /// <param name="newPosition">The new position to check.</param>
    /// <param name="direction">The direction of movement.</param>
    /// <returns>True if the entity can move to the specified position, otherwise false.</returns>
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

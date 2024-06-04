using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Components.Interfaces;

namespace MonoGame;

/// <summary>
/// Represents a game entity in the game world.
/// </summary>
public interface IGameEntity
{
    /// <summary>
    /// Gets or sets the list of components attached to the entity.
    /// </summary>
    List<IEntityComponent> components { get; set; }

    string UUID { get; set; }

    /// <summary>
    /// Gets or sets the position of the entity.
    /// </summary>
    Vector2 Position { get; set; }

    /// <summary>
    /// Gets or sets the speed of the entity.
    /// </summary>
    Vector2 Speed { get; set; }

    /// <summary>
    /// Updates the entity's state based on the specified game time.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    void Update(GameTime gameTime);

    /// <summary>
    /// Draws the entity using the specified sprite batch.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch.</param>
    void Draw(SpriteBatch spriteBatch);

    /// <summary>
    /// Adds a component to the entity.
    /// </summary>
    /// <param name="component">The component to add.</param>
    void AddComponent(IEntityComponent component);

    /// <summary>
    /// Removes a component from the entity.
    /// </summary>
    /// <param name="component">The component to remove.</param>
    void RemoveComponent(IEntityComponent component);

    /// <summary>
    /// Gets the first component of the specified type attached to the entity.
    /// </summary>
    /// <typeparam name="T">The type of component to retrieve.</typeparam>
    /// <returns>The first component of the specified type, or null if not found.</returns>
    T GetFirstComponent<T>();

    /// <summary>
    /// Gets all components of the specified type attached to the entity.
    /// </summary>
    /// <typeparam name="T">The type of components to retrieve.</typeparam>
    /// <returns>A list of components of the specified type.</returns>
    List<T> GetComponents<T>();

    /// <summary>
    /// Determines whether the entity contains a component of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of component to check.</typeparam>
    /// <returns>True if the entity contains a component of the specified type; otherwise, false.</returns>
    bool ContainsComponent<T>();
}

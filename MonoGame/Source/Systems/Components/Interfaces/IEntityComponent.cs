﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Entity.Interfaces;

namespace MonoGame.Source.Systems.Components.Interfaces;

/// <summary>
/// Represents a component that can be attached to a game entity.
/// </summary>
public interface IEntityComponent
{
    /// <summary>
    /// Gets or sets the game entity that this component is attached to.
    /// </summary>
    IGameEntity Entity { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the entity component is initialized.
    /// </summary>
    bool Initialized { get; set; }

    /// <summary>
    /// Initializes the component.
    /// </summary>
    abstract void Initialize();

    /// <summary>
    /// Sets the entity associated with this component.
    /// </summary>
    /// <param name="entity">The game entity to set.</param>
    public abstract void SetEntity(IGameEntity entity);

    /// <summary>
    /// Updates the component.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    public abstract void Update(GameTime gameTime);

    /// <summary>
    /// Draws the component.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used for drawing.</param>
    public abstract void Draw(SpriteBatch spriteBatch);
}

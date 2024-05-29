using System;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Components.Interfaces;
using MonoGame.Source.Systems.Entity.Interfaces;

namespace MonoGame.Source.Systems.Components;

/// <summary>
/// Represents a base class for entity components in a game.
/// </summary>
public abstract class EntityComponent : IEntityComponent
{
    private IGameEntity _entity;

    /// <summary>
    /// Gets or sets the game entity associated with this component.
    /// </summary>
    public IGameEntity Entity
    {
        get => _entity;
        set => _entity = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the entity component is initialized.
    /// </summary>
    public bool Initialized { get; set; } = false;

    /// <summary>
    /// Initializes the entity component with the specified game entity.
    /// </summary>
    /// <param name="entity">The game entity to associate with this component.</param>
    public void BaseInitialize(IGameEntity entity)
    {
        Entity = entity;
    }

    /// <summary>
    /// Draws the entity component using the specified sprite batch.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used for drawing.</param>
    public virtual void Draw(SpriteBatch spriteBatch)
    {

    }

    /// <summary>
    /// Initializes the entity component.
    /// </summary>
    public virtual void Initialize()
    {

    }

    public void SetEntity(IGameEntity entity)
    {
        Entity = entity;
    }

    /// <summary>
    /// Updates the entity component based on the specified game time.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    public virtual void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {

    }
}

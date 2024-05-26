using System;
using MonoGame.Source.Systems.Entity.Interfaces;

namespace MonoGame.Source.Systems.Components.Collision;

/// <summary>
/// Represents a component that handles collision for a game entity.
/// </summary>
public class CollisionComponent
{
    /// <summary>
    /// Gets or sets the game entity associated with this collision component.
    /// </summary>
    public IGameEntity Entity { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CollisionComponent"/> class.
    /// </summary>
    /// <param name="entity">The game entity to associate with this collision component.</param>
    public CollisionComponent(IGameEntity entity)
    {
        Entity = entity;
    }
}

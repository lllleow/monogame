using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Source.Systems.Entity.Interfaces;

namespace MonoGame.Source.Systems.Components.Collision;
/// <summary>
/// Represents a component responsible for handling collision detection and response.
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

    /// <summary>
    /// Gets the list of tiles that are colliding with the specified mask and rectangle.
    /// </summary>
    /// <param name="mask">The collision mask.</param>
    /// <param name="rectangle">The rectangle to check for collision.</param>
    /// <returns>A list of tiles that are colliding with the specified mask and rectangle.</returns>
    public List<ITile> GetTilesCollidingWithMask(bool[,] mask, Rectangle rectangle)
    {
        List<ITile> tiles = Globals.world.GetTilesIntersectingWithMask(mask, rectangle);
        return tiles;
    }

    /// <summary>
    /// Gets the list of tiles that are colliding with the specified rectangle.
    /// </summary>
    /// <param name="rectangle">The rectangle to check for collision.</param>
    /// <returns>A list of tiles that are colliding with the specified rectangle.</returns>
    public List<ITile> GetTilesCollidingWithRectangle(Rectangle rectangle)
    {
        List<ITile> tiles = Globals.world.GetTilesIntersectingWithRectangle(rectangle);
        return tiles;
    }

    /// <summary>
    /// Gets the list of tiles that are colliding with the specified circle.
    /// </summary>
    /// <param name="position">The center position of the circle.</param>
    /// <param name="radius">The radius of the circle.</param>
    /// <returns>A list of tiles that are colliding with the specified circle.</returns>
    public List<ITile> GetTilesCollidingWithCircle(Vector2 position, float radius)
    {
        List<ITile> tiles = Globals.world.GetTilesIntersectingWithCircle(position, radius);
        return tiles;
    }
}

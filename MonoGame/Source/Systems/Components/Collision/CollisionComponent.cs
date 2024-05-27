using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Source.Systems.Components.Interfaces;

namespace MonoGame.Source.Systems.Components.Collision;
/// <summary>
/// Represents a component responsible for handling collision detection and response.
/// </summary>
public class CollisionComponent : EntityComponent
{
    public CollisionMode Mode;
    public string CollisionMaskSpritesheet;

    public CollisionComponent(CollisionMode mode)
    {
        Mode = mode;
    }

    public CollisionComponent(string collisionMaskSpritesheet)
    {

        Mode = CollisionMode.CollisionMask;
        CollisionMaskSpritesheet = collisionMaskSpritesheet;
    }

    /// <summary>
    /// Initializes the AnimatorComponent.
    /// </summary>
    public override void Initialize()
    {
        if (Mode == CollisionMode.BoundingBox && !Entity.ContainsComponent<BoundingBoxComponent>())
        {
            throw new Exception("CollisionComponent in BoundingBox mode requires a BoundingBoxComponent to be present on the entity.");
        }
        else if ((Mode == CollisionMode.CollisionMask || Mode == CollisionMode.PixelPerfect) && !Entity.ContainsComponent<PixelBoundsComponent>())
        {
            throw new Exception("CollisionComponent in CollisionMask or PixelPerfect mode requires a PixelBoundsComponent to be present on the entity.");
        }
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

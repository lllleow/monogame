using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Source.Systems.Components.BoundingBox;
using MonoGame.Source.Systems.Components.Collision.Enum;
using MonoGame.Source.Systems.Components.PixelBounds;
using MonoGame.Source.Systems.Tiles.Interfaces;

namespace MonoGame.Source.Systems.Components.Collision;

public class CollisionComponent : EntityComponent
{
    public CollisionMode Mode { get; set; }
    public string CollisionMaskSpritesheet { get; set; }
    public static bool ShowCollisionMasks { get; set; } = false;

    public CollisionComponent(CollisionMode mode)
    {
        Mode = mode;
    }

    public CollisionComponent(string collisionMaskSpritesheet)
    {
        Mode = CollisionMode.CollisionMask;
        CollisionMaskSpritesheet = collisionMaskSpritesheet;
    }

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

    public List<ITile> GetTilesCollidingWithMask(bool[,] mask, Rectangle rectangle)
    {
        List<ITile> tiles = Globals.World.GetTilesIntersectingWithMask(mask, rectangle);
        return tiles;
    }

    public List<ITile> GetTilesCollidingWithRectangle(Rectangle rectangle)
    {
        List<ITile> tiles = Globals.World.GetTilesIntersectingWithRectangle(rectangle);
        return tiles;
    }

    public List<ITile> GetTilesCollidingWithCircle(Vector2 position, float radius)
    {
        List<ITile> tiles = Globals.World.GetTilesIntersectingWithCircle(position, radius);
        return tiles;
    }
}

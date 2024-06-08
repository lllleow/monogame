using Microsoft.Xna.Framework;
using MonoGame;
using MonoGame_Server.Systems.Server;
using MonoGame.Source.States;
using MonoGame.Source.States.Components;
using MonoGame.Source.Systems.Components.Collision.Enum;
using MonoGame.Source.Systems.Tiles;
using MonoGame.Source.Util.Enum;

namespace MonoGame_Server;

public class ServerMovementHelper
{
    public bool CanMove(EntityState entity, Vector2 newPosition, Direction direction)
    {
        if (entity.HasComponent(typeof(CollisionComponentState)))
        {
            Rectangle entityRectangle = this.GetEntityBoundsAtPosition(entity, newPosition);
            CollisionComponentState collisionComponent = entity.GetComponent<CollisionComponentState>();
            List<TileState> tiles;

            // TODO: Different collision methods
            if (collisionComponent.Mode == CollisionMode.BoundingBox)
            {
                tiles = NetworkServer.Instance.ServerWorld.GetTilesIntersectingWithRectangle(entityRectangle);
            }
            else if (collisionComponent.Mode is CollisionMode.PixelPerfect or CollisionMode.CollisionMask)
            {
                // PixelBoundsComponent pixelBounds = Entity.GetFirstComponent<PixelBoundsComponent>();
                tiles = []; // collisionComponent.GetTilesCollidingWithMask(pixelBounds.Mask, entityRectangle);
            }
            else
            {
                return true;
            }

            return tiles.Count == 0;
        }

        return true;
    }

    public Rectangle GetEntityBoundsAtPosition(EntityState entity, Vector2 position)
    {
        if (entity.HasComponent(typeof(AnimatorComponentState)))
        {
            return new Rectangle((int)position.X, (int)position.Y, Tile.PixelSizeX, Tile.PixelSizeY);
        }
        else
        {
            return Rectangle.Empty;
        }
    }
}

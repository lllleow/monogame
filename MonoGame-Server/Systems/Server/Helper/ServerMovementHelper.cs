using System.Drawing;
using System.Numerics;
using MonoGame_Common.Enums;
using MonoGame_Common.States;
using MonoGame_Common.States.Components;
using MonoGame_Common.Util.Enum;

namespace MonoGame_Server.Systems.Server.Helper;

public class ServerMovementHelper
{
    public bool CanMove(EntityState entity, Vector2 newPosition, Direction direction)
    {
        if (entity.HasComponent(typeof(CollisionComponentState)))
        {
            var entityRectangle = GetEntityBoundsAtPosition(entity, newPosition);
            var collisionComponent = entity.GetComponent<CollisionComponentState>();
            List<TileState> tiles = [];

            if (collisionComponent.Mode == CollisionMode.BoundingBox)
            {
                tiles = NetworkServer.Instance.ServerWorld.GetTilesIntersectingWithRectangle(entityRectangle);
            }
            else if (collisionComponent.Mode is CollisionMode.CollisionMask)
            {
                if (entity.HasComponent(typeof(AnimatorComponentState)))
                {
                    // var animator = entity.GetComponent<AnimatorComponentState>();
                    // IAnimationBundle animationBundle = AnimationBundleRegistry.GetAnimationBundle(animator.AnimationBundleId);
                    // Animation currentAnimation = animationBundle.Animations[animator.CurrentState];
                    // var animationState = new AnimationState(currentAnimation, animationBundle)
                    // {
                    //     CurrentTime = animator.CurrentTime
                    // };
                    // Rectangle textureRectangle = animationState.GetTextureRectangle();
                    // bool[,] mask = CollisionMaskHandler.GetMaskForTexture(animationBundle.CollisionMaskSpritesheet, textureRectangle);
                    // tiles = NetworkServer.Instance.ServerWorld.GetTilesIntersectingWithMask(mask, entityRectangle);
                }
                else
                {
                    tiles = NetworkServer.Instance.ServerWorld.GetTilesIntersectingWithRectangle(entityRectangle);
                }
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
        return entity.HasComponent(typeof(AnimatorComponentState))
            ? new Rectangle((int)position.X, (int)position.Y, TileState.PixelSizeX, TileState.PixelSizeY)
            : Rectangle.Empty;
    }
}
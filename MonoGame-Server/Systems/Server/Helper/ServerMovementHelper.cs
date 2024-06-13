using System.Numerics;
using MonoGame_Common;
using MonoGame_Common.Enums;
using MonoGame_Common.States;
using MonoGame_Common.States.Components;
using MonoGame_Common.Systems.Animation;
using MonoGame_Common.Systems.Scripts;
using MonoGame_Common.Util.Enum;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace MonoGame_Server.Systems.Server.Helper;

public class ServerMovementHelper
{
    public bool CanMove(EntityState entity, Vector2 newPosition)
    {
        if (entity.HasComponent(typeof(CollisionComponentState)))
        {
            System.Drawing.Rectangle entityRectangle = GetEntityBoundsAtPosition(entity, newPosition);
            CollisionComponentState collisionComponent = entity.GetComponent<CollisionComponentState>();
            List<PositionedTileHelper> tiles = [];

            if (collisionComponent.Mode == CollisionMode.BoundingBox)
            {
                tiles = NetworkServer.Instance.ServerWorld.GetTilesIntersectingWithRectangle(entityRectangle);
            }
            else if (collisionComponent.Mode is CollisionMode.CollisionMask)
            {
                if (entity.HasComponent(typeof(AnimatorComponentState)))
                {
                    AnimatorComponentState animator = entity.GetComponent<AnimatorComponentState>();
                    IAnimationBundle? animationBundle = AnimationBundleRegistry.GetAnimationBundle(animator.AnimationBundleId);
                    Animation? currentAnimation = animationBundle?.Animations[animator.CurrentState];
                    if (currentAnimation != null && animationBundle != null)
                    {
                        var animationState = new AnimationState(currentAnimation, animationBundle)
                        {
                            CurrentTime = animator.CurrentTime
                        };

                        System.Drawing.Rectangle textureRectangle = animationState.GetTextureRectangle();
                        Image<Rgba32> croppedImage = ServerTextureHelper.GetImageInRectangle(animationBundle.CollisionMaskSpritesheet, textureRectangle);
                        bool[,] mask = ServerTextureHelper.GetImageMask(croppedImage);
                        tiles = NetworkServer.Instance.ServerWorld.GetTilesIntersectingWithMask(mask, entityRectangle);
                    }
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

    public System.Drawing.Rectangle GetEntityBoundsAtPosition(EntityState entity, Vector2 position)
    {
        return entity.HasComponent(typeof(AnimatorComponentState))
            ? new System.Drawing.Rectangle((int)position.X, (int)position.Y, SharedGlobals.PixelSizeX, SharedGlobals.PixelSizeY)
            : System.Drawing.Rectangle.Empty;
    }
}
using Microsoft.Xna.Framework;
using MonoGame;
using MonoGame_Server.Systems.Server;
using MonoGame.Source.States;
using MonoGame.Source.States.Components;
using MonoGame.Source.Systems.Animation;
using MonoGame.Source.Systems.Components.Collision;
using MonoGame.Source.Systems.Components.Collision.Enum;
using MonoGame.Source.Systems.Scripts;
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

            if (collisionComponent.Mode == CollisionMode.BoundingBox)
            {
                tiles = NetworkServer.Instance.ServerWorld.GetTilesIntersectingWithRectangle(entityRectangle);
            }
            else if (collisionComponent.Mode is CollisionMode.CollisionMask)
            {
                if (entity.HasComponent(typeof(AnimatorComponentState)))
                {
                    AnimatorComponentState animator = entity.GetComponent<AnimatorComponentState>();
                    IAnimationBundle animationBundle = AnimationBundleRegistry.GetAnimationBundle(animator.AnimationBundleId);
                    Animation currentAnimation = animationBundle.Animations[animator.CurrentState];
                    AnimationState animationState = new AnimationState(currentAnimation, animationBundle)
                    {
                        CurrentTime = animator.CurrentTime
                    };
                    Rectangle textureRectangle = animationState.GetTextureRectangle();
                    bool[,] mask = CollisionMaskHandler.GetMaskForTexture(animationBundle.CollisionMaskSpritesheet, textureRectangle);
                    tiles = NetworkServer.Instance.ServerWorld.GetTilesIntersectingWithMask(mask, entityRectangle);
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

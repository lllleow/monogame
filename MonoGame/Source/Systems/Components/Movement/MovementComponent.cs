using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Source.Multiplayer;
using MonoGame.Source.Multiplayer.Messages.Player;
using MonoGame.Source.Systems.Components;
using MonoGame.Source.Systems.Components.Animator;
using MonoGame.Source.Systems.Components.Collision;
using MonoGame.Source.Systems.Components.Collision.Enum;
using MonoGame.Source.Systems.Components.PixelBounds;
using MonoGame.Source.Systems.Entity.PlayerNamespace;
using MonoGame.Source.Systems.Tiles.Interfaces;
using MonoGame.Source.Util.Enum;

namespace MonoGame;

public class MovementComponent : EntityComponent
{
    public Vector2 Speed { get; set; } = new(1, 1);

    public override void Update(GameTime gameTime)
    {
        var state = Keyboard.GetState();

        if (state.IsKeyDown(Keys.W))
        {
            NetworkClient.SendMessage(new RequestMovementNetworkMessage(Speed, Direction.Up));
        }

        if (state.IsKeyDown(Keys.A))
        {
            NetworkClient.SendMessage(new RequestMovementNetworkMessage(Speed, Direction.Left));
        }

        if (state.IsKeyDown(Keys.S))
        {
            NetworkClient.SendMessage(new RequestMovementNetworkMessage(Speed, Direction.Down));
        }

        if (state.IsKeyDown(Keys.D))
        {
            NetworkClient.SendMessage(new RequestMovementNetworkMessage(Speed, Direction.Right));
        }
    }

    public void Move(GameTime gameTime, Direction direction, Vector2 speed)
    {
        if (Entity is Player)
        {
            Vector2 displacement = MovementHelper.GetDisplacement(direction, speed);
            Vector2 newPosition = Entity.Position + displacement;

            if (Entity.ContainsComponent<AnimatorComponent>())
            {
                AnimatorComponent animator = Entity.GetFirstComponent<AnimatorComponent>();

                switch (direction)
                {
                    case Direction.Up:
                        animator.PlayAnimation("walking_back");
                        break;
                    case Direction.Down:
                        animator.PlayAnimation("walking_front");
                        break;
                    case Direction.Left:
                        animator.PlayAnimation("walking_left");
                        break;
                    case Direction.Right:
                        animator.PlayAnimation("walking_right");
                        break;
                    default:
                        animator.PlayAnimation("idle");
                        break;
                }
            }

            Entity.Position = newPosition;
        }
    }

    public bool CanMove(Vector2 newPosition, Direction direction)
    {
        if (Entity.ContainsComponent<CollisionComponent>())
        {
            Rectangle entityRectangle = Entity.GetEntityBoundsAtPosition(newPosition);
            CollisionComponent collisionComponent = Entity.GetFirstComponent<CollisionComponent>();
            List<ITile> tiles;

            if (collisionComponent.Mode == CollisionMode.BoundingBox)
            {
                tiles = collisionComponent.GetTilesCollidingWithRectangle(entityRectangle);
            }
            else if (collisionComponent.Mode is CollisionMode.PixelPerfect or CollisionMode.CollisionMask)
            {
                PixelBoundsComponent pixelBounds = Entity.GetFirstComponent<PixelBoundsComponent>();
                tiles = collisionComponent.GetTilesCollidingWithMask(pixelBounds.Mask, entityRectangle);
            }
            else
            {
                return true;
            }

            return tiles.Count == 0;
        }

        return true;
    }
}

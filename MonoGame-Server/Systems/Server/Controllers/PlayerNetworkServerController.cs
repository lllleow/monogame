using System.Numerics;
using MonoGame_Common;
using MonoGame_Common.Enums;
using MonoGame_Common.Messages.Components.Animator;
using MonoGame_Common.Messages.Player;
using MonoGame_Common.Util.Enum;
using MonoGame_Common.Util.Helpers;
using MonoGame_Server.Systems.Server.Helper;

namespace MonoGame_Server.Systems.Server.Controllers;

public class PlayerNetworkServerController : IServerNetworkController
{
    private ServerMovementHelper ServerMovementHelper { get; } = new();
    private Vector2 SpawnPosition { get; } = new(128, 128);

    public void InitializeListeners()
    {
        ServerNetworkEventManager.Subscribe<KeyClickedNetworkMessage>((server, peer, message) =>
        {
            var playerState = server.GetPlayerFromPeer(peer);
            if (playerState == null) return;

            if (message.Keys.Contains(Keys.W) || message.Keys.Contains(Keys.A) || message.Keys.Contains(Keys.S) || message.Keys.Contains(Keys.D))
            {
                playerState.MovementDirection = Direction.None;
                Direction direction = Direction.None;
                foreach (var key in message.Keys)
                {
                    switch (key)
                    {
                        case Keys.W:
                            direction = Direction.Up;
                            break;
                        case Keys.A:
                            direction = Direction.Left;
                            break;
                        case Keys.S:
                            direction = Direction.Down;
                            break;
                        case Keys.D:
                            direction = Direction.Right;
                            break;
                    }
                }

                playerState.MovementDirection = direction;
            }

            if (message.Keys.Count == 0)
            {
                playerState.IsMoving = false;
            }
            else
            {
                playerState.IsMoving = true;
            }

            NetworkServer.Instance.SetEntity(playerState);
        });
    }

    public void Update()
    {
        foreach (var player in NetworkServer.Instance.ServerWorld.Players ?? [])
        {
            if (player.UUID == null) continue;

            if (player.IsMoving)
            {
                Vector2 currentPosition = player.Position;
                Vector2 newPosition = currentPosition + MovementHelper.GetDisplacement(player.MovementDirection, SharedGlobals.PlayerSpeed);

                if (ServerMovementHelper.CanMove(player, newPosition))
                {
                    switch (player.MovementDirection)
                    {
                        case Direction.Up:
                            NetworkServer.Instance.BroadcastMessage(new UpdateAnimatorStateNetworkMessage(player.UUID, "walking_back"));
                            break;
                        case Direction.Left:
                            NetworkServer.Instance.BroadcastMessage(new UpdateAnimatorStateNetworkMessage(player.UUID, "walking_left"));
                            break;
                        case Direction.Down:
                            NetworkServer.Instance.BroadcastMessage(new UpdateAnimatorStateNetworkMessage(player.UUID, "walking_front"));
                            break;
                        case Direction.Right:
                            NetworkServer.Instance.BroadcastMessage(new UpdateAnimatorStateNetworkMessage(player.UUID, "walking_right"));
                            break;
                    }

                    player.LastStateWasIdle = false;
                    player.Position = newPosition;
                    NetworkServer.Instance.BroadcastMessage(new UpdatePlayerPositionNetworkMessage(player.UUID, player.Position));
                }
            }
            else
            {
                if (!player.LastStateWasIdle)
                {
                    NetworkServer.Instance.BroadcastMessage(new UpdateAnimatorStateNetworkMessage(player.UUID, "idle"));
                    player.LastStateWasIdle = true;
                }
            }
        }
    }
}
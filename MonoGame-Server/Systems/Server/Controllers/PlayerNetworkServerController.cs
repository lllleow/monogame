using System.Numerics;
using MonoGame;
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
    public Dictionary<string, Vector2> LevelEditorCameraPositions { get; set; } = new();

    public void InitializeListeners()
    {
        ServerNetworkEventManager.Subscribe<ChangeGameModeNetworkMessage>((server, peer, message) =>
        {
            var playerState = server.GetPlayerFromPeer(peer);
            if (playerState == null) return;
            playerState.GameMode = message.DesiredGameMode;

            server.BroadcastMessage(new SetGameModeNetworkMessage()
            {
                UUID = playerState.UUID,
                GameMode = playerState.GameMode
            });
        });

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
                if (player.GameMode == GameMode.Survival)
                {
                    Vector2 currentPosition = player.Position;
                    Vector2 newPosition = currentPosition + MovementHelper.GetDisplacement(player.MovementDirection, SharedGlobals.PlayerSpeed);

                    if (ServerMovementHelper.CanMove(player, newPosition))
                    {
                        string targetState = "walking_front";
                        switch (player.MovementDirection)
                        {
                            case Direction.Up:
                                targetState = "walking_back";
                                break;
                            case Direction.Left:
                                targetState = "walking_left";
                                break;
                            case Direction.Down:
                                targetState = "walking_front";
                                break;
                            case Direction.Right:
                                targetState = "walking_right";
                                break;
                        }

                        NetworkServer.Instance.BroadcastMessage(new UpdateAnimatorStateNetworkMessage()
                        {
                            UUID = player.UUID,
                            TargetState = targetState
                        });

                        player.LastStateWasIdle = false;
                        player.Position = newPosition;
                        NetworkServer.Instance.BroadcastMessage(new UpdatePlayerPositionNetworkMessage()
                        {
                            UUID = player.UUID,
                            Position = player.Position
                        });
                    }
                }
                else if (player.GameMode == GameMode.LevelEditor)
                {
                    if (!LevelEditorCameraPositions.ContainsKey(player.UUID))
                    {
                        LevelEditorCameraPositions.Add(player.UUID, player.Position);
                    }

                    Vector2 currentPosition = LevelEditorCameraPositions[player.UUID];
                    Vector2 newPosition = currentPosition + MovementHelper.GetDisplacement(player.MovementDirection, SharedGlobals.PlayerSpeed * 3);
                    if (newPosition != currentPosition)
                    {
                        LevelEditorCameraPositions[player.UUID] = newPosition;

                        NetworkServer.Instance.SendMessage(player.UUID, new SetLevelEditorCameraPositionNetworkMessage()
                        {
                            UUID = player.UUID,
                            Position = newPosition
                        });
                    }
                }
            }
            else
            {
                if (!player.LastStateWasIdle)
                {
                    NetworkServer.Instance.BroadcastMessage(new UpdateAnimatorStateNetworkMessage()
                    {
                        UUID = player.UUID,
                        TargetState = "idle"
                    });
                    player.LastStateWasIdle = true;
                }
            }
        }
    }
}
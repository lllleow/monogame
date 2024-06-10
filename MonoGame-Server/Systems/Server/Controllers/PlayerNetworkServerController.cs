using System.Numerics;
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

            if (message.Keys.Contains(Keys.W) || message.Keys.Contains(Keys.A) || message.Keys.Contains(Keys.S) ||
                message.Keys.Contains(Keys.D))
            {
                var resultingDisplacement = Vector2.Zero;
                foreach (var key in message.Keys)
                {
                    switch (key)
                    {
                        case Keys.W:
                            resultingDisplacement += MovementHelper.GetDisplacement(Direction.Up, new Vector2(1, 1));
                            break;
                        case Keys.A:
                            resultingDisplacement += MovementHelper.GetDisplacement(Direction.Left, new Vector2(1, 1));
                            break;
                        case Keys.S:
                            resultingDisplacement += MovementHelper.GetDisplacement(Direction.Down, new Vector2(1, 1));
                            break;
                        case Keys.D:
                            resultingDisplacement += MovementHelper.GetDisplacement(Direction.Right, new Vector2(1, 1));
                            break;
                    }
                }

                var newPosition = (playerState?.Position ?? SpawnPosition) + resultingDisplacement;
                var direction =
                    DirectionHelper.GetDirection((int)resultingDisplacement.X, (int)resultingDisplacement.Y);

                if (!ServerMovementHelper.CanMove(playerState!, newPosition, direction)) return;

                if (playerState?.Position != null) playerState.Position = newPosition;

                switch (direction)
                {
                    case Direction.Up:
                        server.BroadcastMessage(
                            new UpdateAnimatorStateNetworkMessage(playerState?.UUID, "walking_front"));
                        break;
                    case Direction.Left:
                        server.BroadcastMessage(
                            new UpdateAnimatorStateNetworkMessage(playerState?.UUID, "walking_left"));
                        break;
                    case Direction.Down:
                        server.BroadcastMessage(
                            new UpdateAnimatorStateNetworkMessage(playerState?.UUID, "walking_back"));
                        break;
                    case Direction.Right:
                        server.BroadcastMessage(
                            new UpdateAnimatorStateNetworkMessage(playerState?.UUID, "walking_right"));
                        break;
                }

                server.BroadcastMessage(new UpdatePlayerPositionNetworkMessage(playerState?.UUID, newPosition));
            }

            if (message.Keys.Count == 0)
                server.BroadcastMessage(new UpdateAnimatorStateNetworkMessage(playerState?.UUID, "idle"));
        });
    }
}
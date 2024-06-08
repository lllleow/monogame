using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using MonoGame.Source;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.Messages.Player;
using MonoGame.Source.Util.Enum;

namespace MonoGame_Server;

public class PlayerNetworkServerController : IStandaloneNetworkController
{
    public void InitializeListeners()
    {
        ServerNetworkEventManager.Subscribe<KeyClickedNetworkMessage>((server, peer, message) =>
        {
            var playerState = server.GetPlayerFromPeer(peer);
            if (message.Keys.Contains(Keys.W) || message.Keys.Contains(Keys.A) || message.Keys.Contains(Keys.S) || message.Keys.Contains(Keys.D))
            {
                Vector2 resultingDisplacement = Vector2.Zero;
                foreach (Keys key in message.Keys)
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

                var newPosition = (playerState?.Position ?? Globals.SpawnPosition) + resultingDisplacement;
                if (playerState?.Position != null)
                {
                    playerState.Position = newPosition;
                }

                Direction direction = DirectionHelper.GetDirection((int)resultingDisplacement.X, (int)resultingDisplacement.Y);
                switch (direction)
                {
                    case Direction.Up:
                        server.BroadcastMessage(new UpdateAnimatorStateNetworkMessage(playerState?.UUID, "walking_front"));
                        break;
                    case Direction.Left:
                        server.BroadcastMessage(new UpdateAnimatorStateNetworkMessage(playerState?.UUID, "walking_left"));
                        break;
                    case Direction.Down:
                        server.BroadcastMessage(new UpdateAnimatorStateNetworkMessage(playerState?.UUID, "walking_back"));
                        break;
                    case Direction.Right:
                        server.BroadcastMessage(new UpdateAnimatorStateNetworkMessage(playerState?.UUID, "walking_right"));
                        break;
                }

                server.BroadcastMessage(new UpdatePlayerPositionNetworkMessage(playerState?.UUID, newPosition));
            }

            if (message.Keys.Count == 0)
            {
                server.BroadcastMessage(new UpdateAnimatorStateNetworkMessage(playerState?.UUID, "idle"));
            }
        });
    }
}

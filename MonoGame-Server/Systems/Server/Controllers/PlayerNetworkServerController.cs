using MonoGame;
using MonoGame.Source;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.Messages.Player;

namespace MonoGame_Server;

public class PlayerNetworkServerController : IStandaloneNetworkController
{
    public void InitializeListeners()
    {
        ServerNetworkEventManager.Subscribe<RequestMovementNetworkMessage>((server, peer, message) =>
        {
            var playerState = server.GetPlayerFromPeer(peer);

            var displacement = MovementHelper.GetDisplacement(message.Direction, message.Speed);
            var newPosition = (playerState?.Position ?? Globals.SpawnPosition) + displacement;
            if (playerState != null)
            {
                playerState.Position = newPosition;
            }

            server.BroadcastMessage(new MovePlayerNetworkMessage(playerState?.UUID, message.Speed, message.Direction, newPosition));
        });
    }
}

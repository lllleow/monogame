using Microsoft.Xna.Framework;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessageHandler;

namespace MonoGame;

public class MovePlayerNetworkMessageHandler : IClientMessageHandler
{
    public void Execute(byte channel, INetworkMessage message)
    {
        MovePlayerNetworkMessage allowMovementNetworkMessage = (MovePlayerNetworkMessage)message;
        Player player = Globals.World.GetPlayerByUUID(allowMovementNetworkMessage.UUID);

        if (Vector2.Distance(player.Position, allowMovementNetworkMessage.ExpectedPosition) < 1f)
        {
            player.Position = allowMovementNetworkMessage.ExpectedPosition;
        }
        else
        {
            player.Move(Globals.gameTime, allowMovementNetworkMessage.Direction, allowMovementNetworkMessage.Speed);
        }
    }
}

using Microsoft.Xna.Framework;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessageHandler;

namespace MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessageHandler.Client;

public class MovePlayerNetworkMessageHandler : IClientMessageHandler
{
    public void Execute(byte channel, INetworkMessage message)
    {
        var allowMovementNetworkMessage = (MovePlayerNetworkMessage)message;
        var player = Globals.World.GetPlayerByUUID(allowMovementNetworkMessage.UUID);

        if (Vector2.Distance(player.Position, allowMovementNetworkMessage.ExpectedPosition) < 1f)
        {
            player.Position = allowMovementNetworkMessage.ExpectedPosition;
        }
        else
        {
            player.Move(Globals.GameTime, allowMovementNetworkMessage.Direction, allowMovementNetworkMessage.Speed);
        }
    }
}

using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessageHandler;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Server;

namespace MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessageHandler.Client;

public class UpdatePlayerPositionNetworkMessageHandler : IClientMessageHandler
{
    public void Execute(byte channel, INetworkMessage message)
    {
        var updatePlayerPositionNetworkMessage = (UpdatePlayerPositionNetworkMessage)message;
        Globals.World.GetPlayerByUUID(updatePlayerPositionNetworkMessage.UUID).Position = updatePlayerPositionNetworkMessage.Position;
    }
}

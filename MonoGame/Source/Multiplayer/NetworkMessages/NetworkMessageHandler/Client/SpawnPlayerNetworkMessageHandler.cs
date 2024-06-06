using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessageHandler;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Server;
using MonoGame.Source.Systems.Entity.PlayerNamespace;

namespace MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessageHandler.Client;

public class SpawnPlayerNetworkMessageHandler : IClientMessageHandler
{
    public void Execute(byte channel, INetworkMessage message)
    {
        var spawnPlayerNetworkMessage = (SpawnPlayerNetworkMessage)message;

        var player = new Player(spawnPlayerNetworkMessage.UUID, spawnPlayerNetworkMessage.Position);
        Globals.World.Players.Add(player);
    }
}

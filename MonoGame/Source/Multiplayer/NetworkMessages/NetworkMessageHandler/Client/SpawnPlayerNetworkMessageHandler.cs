using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessageHandler;

namespace MonoGame;

public class SpawnPlayerNetworkMessageHandler : IClientMessageHandler
{
    public void Execute(byte channel, INetworkMessage message)
    {
        SpawnPlayerNetworkMessage spawnPlayerNetworkMessage = (SpawnPlayerNetworkMessage)message;

        Player player = new Player(spawnPlayerNetworkMessage.UUID, spawnPlayerNetworkMessage.Position);
        Globals.World.Players.Add(player);
    }
}

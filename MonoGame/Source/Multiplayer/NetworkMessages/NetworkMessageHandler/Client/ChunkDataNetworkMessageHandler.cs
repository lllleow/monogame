using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessageHandler;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Server;

namespace MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessageHandler.Client;

public class ChunkDataNetworkMessageHandler : IClientMessageHandler
{
    public void Execute(byte channel, INetworkMessage message)
    {
        var chunkDataNetworkMessage = (ChunkDataNetworkMessage)message;
        Globals.World.LoadChunkFromChunkState(chunkDataNetworkMessage.ChunkState);
    }
}
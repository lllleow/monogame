using System;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessageHandler;

namespace MonoGame;

public class ChunkDataNetworkMessageHandler : IClientMessageHandler
{
    public void Execute(byte channel, INetworkMessage message)
    {
        ChunkDataNetworkMessage chunkDataNetworkMessage = (ChunkDataNetworkMessage)message;
        Globals.world.LoadChunkFromChunkState(chunkDataNetworkMessage.ChunkState);
    }
}
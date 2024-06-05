using LiteNetLib;
using Microsoft.Xna.Framework;
using MonoGame;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Client;
using MonoGame.Source.Systems.Chunks.Interfaces;
using MonoGame_Server.Systems.Server;
using MonoGame_Server.Systems.Server.MessageHandlers;

namespace MonoGame_Server;

public class RequestToLoadWorldServerMessageHandler : IServerMessageHandler
{
    NetworkServer NetworkServer = NetworkServer.Instance;

    public bool Validate(NetPeer peer, byte channel, INetworkMessage message)
    {
        return true;
    }

    public void Execute(NetPeer peer, byte channel, INetworkMessage message)
    {
        foreach (ChunkState chunk in NetworkServer.ServerWorld.Chunks!)
        {
            ChunkDataNetworkMessage chunkDataNetworkMessage = new ChunkDataNetworkMessage(chunk);
            NetworkServer.SendMessageToPeer(peer, chunkDataNetworkMessage);
        }

        string uuid = NetworkServer.GetUUIDByPeer(peer);

        foreach (PlayerState playerState in NetworkServer.ServerWorld.Players!)
        {
            SpawnPlayerNetworkMessage spawnPlayerNetworkMessage = new SpawnPlayerNetworkMessage(playerState.UUID, playerState.Position ?? Globals.spawnPosition);
            NetworkServer.SendMessageToPeer(peer, spawnPlayerNetworkMessage);
        }

        PlayerState existingPlayer = NetworkServer.ServerWorld.Players.FirstOrDefault(p => p.UUID == uuid);
        if (existingPlayer == null)
        {
            PlayerState newPlayer = new PlayerState(uuid);
            NetworkServer.ServerWorld.Players.Add(newPlayer);
            SpawnPlayerNetworkMessage spawnPlayerNetworkMessage = new SpawnPlayerNetworkMessage(newPlayer.UUID, newPlayer.Position ?? Globals.spawnPosition);
            NetworkServer.BroadcastMessage(spawnPlayerNetworkMessage);
        }
    }
}

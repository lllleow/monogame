using LiteNetLib;
using MonoGame.Source;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Server;
using MonoGame.Source.WorldNamespace.WorldStates;

namespace MonoGame_Server.Systems.Server.MessageHandlers;

public class RequestToLoadWorldServerMessageHandler : IServerMessageHandler
{
    private readonly NetworkServer networkServer = NetworkServer.Instance;

    public bool Validate(NetPeer peer, byte channel, INetworkMessage message)
    {
        return true;
    }

    public void Execute(NetPeer peer, byte channel, INetworkMessage message)
    {
        foreach (var chunk in networkServer.ServerWorld.Chunks!)
        {
            var chunkDataNetworkMessage = new ChunkDataNetworkMessage(chunk);
            networkServer.SendMessageToPeer(peer, chunkDataNetworkMessage);
        }

        foreach (var uuid in networkServer.Connections.Values)
        {
            var playerState = networkServer.ServerWorld.Players?.FirstOrDefault(p => p.UUID == uuid);
            if (playerState != null)
            {
                var spawnPlayerNetworkMessage = new SpawnPlayerNetworkMessage(playerState.UUID, playerState.Position ?? Globals.SpawnPosition);
                networkServer.SendMessageToPeer(peer, spawnPlayerNetworkMessage);
            }
        }

        var existingPlayerUUID = networkServer.GetUUIDByPeer(peer);
        var existingPlayer = networkServer.ServerWorld.Players?.FirstOrDefault(p => p.UUID == existingPlayerUUID);
        if (existingPlayer == null)
        {
            var newPlayer = new PlayerState(existingPlayerUUID);
            networkServer.ServerWorld.Players?.Add(newPlayer);
            var spawnPlayerNetworkMessage = new SpawnPlayerNetworkMessage(newPlayer.UUID, newPlayer.Position ?? Globals.SpawnPosition);
            networkServer.BroadcastMessage(spawnPlayerNetworkMessage);
        }
    }
}

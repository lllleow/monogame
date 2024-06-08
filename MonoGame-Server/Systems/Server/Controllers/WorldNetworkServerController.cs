using MonoGame;
using MonoGame.Source;
using MonoGame.Source.Multiplayer.Interfaces;
using MonoGame.Source.Multiplayer.Messages.Player;
using MonoGame.Source.Multiplayer.Messages.World;
using MonoGame.Source.States;

namespace MonoGame_Server;

public class WorldNetworkServerController : IStandaloneNetworkController
{
    public void InitializeListeners()
    {
        ServerNetworkEventManager.Subscribe<RequestToLoadWorldNetworkMessage>((server, peer, message) =>
        {
            foreach (var chunk in server.ServerWorld.Chunks!)
            {
                var chunkDataNetworkMessage = new ChunkDataNetworkMessage(chunk);
                server.SendMessageToPeer(peer, chunkDataNetworkMessage);
            }

            foreach (var uuid in server.Connections.Values)
            {
                var playerState = server.ServerWorld.Players?.FirstOrDefault(p => p.UUID == uuid);
                if (playerState != null)
                {
                    var spawnPlayerNetworkMessage = new SpawnPlayerNetworkMessage(playerState.UUID, playerState.Position ?? Globals.SpawnPosition);
                    server.SendMessageToPeer(peer, spawnPlayerNetworkMessage);
                }
            }

            var existingPlayerUUID = server.GetUUIDByPeer(peer);
            var existingPlayer = server.ServerWorld.Players?.FirstOrDefault(p => p.UUID == existingPlayerUUID);
            if (existingPlayer == null)
            {
                var newPlayer = new PlayerState(existingPlayerUUID);
                server.ServerWorld.Players?.Add(newPlayer);
                var spawnPlayerNetworkMessage = new SpawnPlayerNetworkMessage(newPlayer.UUID, newPlayer.Position ?? Globals.SpawnPosition);
                server.BroadcastMessage(spawnPlayerNetworkMessage);
            }
        });

        ServerNetworkEventManager.Subscribe<RequestToPlaceTileNetworkMessage>((server, peer, message) =>
        {
            var tile = server.ServerWorld.GetTileAtPosition(message.Layer, message.PosX, message.PosY);
            if (tile == null)
            {
                server.ServerWorld.SetTileAtPosition(message.TileId, message.Layer, message.PosX, message.PosY);
            }
            else
            {
                server.ServerWorld.DestroyTileAtPosition(message.Layer, message.PosX, message.PosY);
            }
        });
    }
}

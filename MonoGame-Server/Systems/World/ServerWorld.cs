using Microsoft.Xna.Framework;
using MonoGame_Server.Systems.Saving;
using MonoGame_Server.Systems.Server;
using MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Server;
using MonoGame.Source.Rendering.Enum;
using MonoGame.Source.Systems.Chunks;
using MonoGame.Source.WorldNamespace.WorldStates;

namespace MonoGame_Server.Systems.World;

public class ServerWorld
{
    public List<ChunkState>? Chunks { get; set; }
    public List<PlayerState>? Players { get; set; }
    public List<EntityState>? Entities { get; set; }
    private readonly SaveManager saveManager = new();

    public void Initialize()
    {
        var worldState = saveManager.LoadGame("C:\\Users\\Leonardo\\Documents\\Repositories\\monogame\\save\\");

        Chunks = worldState.Chunks ?? [];
        Players = worldState.Players ?? [];
        Entities = worldState.Entities ?? [];
    }

    public (List<PlayerState>? Players, List<ChunkState>? Chunks, List<EntityState>? Entities) GetWorldState()
    {
        return (Players, Chunks, Entities);
    }

    public PlayerState? GetPlayerByUUID(string UUID)
    {
        return Players?.FirstOrDefault(x => x.UUID == UUID);
    }

    public TileState? GetTileAtPosition(TileDrawLayer layer, int posX, int posY)
    {
        var chunkX = posX / Chunk.SizeX;
        var chunkY = posY / Chunk.SizeY;
        var localX = posX % Chunk.SizeX;
        var localY = posY % Chunk.SizeY;

        var chunk = Chunks?.FirstOrDefault(x => x.X == chunkX && x.Y == chunkY);
        return chunk?.GetTile(layer, localX, localY);
    }

    public void DestroyTileAtPosition(TileDrawLayer layer, int posX, int posY)
    {
        var chunkX = posX / Chunk.SizeX;
        var chunkY = posY / Chunk.SizeY;
        var localX = posX % Chunk.SizeX;
        var localY = posY % Chunk.SizeY;

        var chunk = Chunks?.FirstOrDefault(x => x.X == chunkX && x.Y == chunkY);
        var removedAny = chunk?.DestroyTile(layer, localX, localY);
        if (removedAny ?? true)
        {
            NetworkServer.Instance.BroadcastMessage(new DeleteTileNetworkMessage(layer, posX, posY));
            SaveManager.SaveGame("C:\\Users\\Leonardo\\Documents\\Repositories\\monogame\\save\\");
        }
    }

    public void SetTileAtPosition(string tileId, TileDrawLayer layer, int posX, int posY)
    {
        var worldPosition = new Vector2(posX, posY);

        var chunkX = posX / Chunk.SizeX;
        var chunkY = posY / Chunk.SizeY;
        var localX = posX % Chunk.SizeX;
        var localY = posY % Chunk.SizeY;

        var chunk = Chunks?.FirstOrDefault(x => x.X == chunkX && x.Y == chunkY);

        if (chunk == null)
        {
            var newChunk = new ChunkState(chunkX, chunkY);
            chunk = newChunk;
            Chunks?.Add(newChunk);
        }

        var success = chunk.SetTile(tileId, layer, localX, localY);
        if (success)
        {
            NetworkServer.Instance.BroadcastMessage(new PlaceTileNetworkMessage(tileId, layer, posX, posY));
            SaveManager.SaveGame("C:\\Users\\Leonardo\\Documents\\Repositories\\monogame\\save\\");
        }
    }
}

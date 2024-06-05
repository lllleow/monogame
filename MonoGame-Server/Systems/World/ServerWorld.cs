using Microsoft.Xna.Framework;
using MonoGame;
using MonoGame.Source.Systems.Chunks;
using MonoGame_Server.Systems.Server;

namespace MonoGame_Server;

public class ServerWorld
{
    public List<ChunkState>? Chunks;
    public List<PlayerState>? Players;
    public List<EntityState>? Entities;
    private SaveManager SaveManager = new SaveManager();

    public void Initialize()
    {
        (List<PlayerState>?, List<ChunkState>?, List<EntityState>?) worldState = SaveManager.LoadGame("C:\\Users\\Leonardo\\Documents\\Repositories\\monogame\\save\\");

        Chunks = worldState.Item2 ?? new List<ChunkState>();
        Players = worldState.Item1 ?? new List<PlayerState>();
        Entities = worldState.Item3 ?? new List<EntityState>();
    }

    public (List<PlayerState>?, List<ChunkState>?, List<EntityState>?) GetWorldState()
    {
        return (Players, Chunks, Entities);
    }

    public PlayerState GetPlayerByUUID(string UUID)
    {
        return Players.FirstOrDefault(x => x.UUID == UUID);
    }

    public void SetTileAtPosition(string tileId, TileDrawLayer layer, int posX, int posY)
    {
        Vector2 worldPosition = new Vector2(posX, posY);

        int chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        int chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        int chunkX = (int)(worldPosition.X / chunkSizeInPixelsX);
        int chunkY = (int)(worldPosition.Y / chunkSizeInPixelsY);

        int localX = (int)(worldPosition.X % chunkSizeInPixelsX) / Tile.PixelSizeX;
        int localY = (int)(worldPosition.Y % chunkSizeInPixelsY) / Tile.PixelSizeY;

        ChunkState chunk = Chunks.FirstOrDefault(x => x.X == chunkX && x.Y == chunkY);

        if (chunk == null)
        {
            ChunkState newChunk = new ChunkState(chunkX, chunkY);
            chunk = newChunk;
            Chunks.Add(newChunk);
        }

        bool success = chunk.SetTile(tileId, layer, localX, localY);
        if (success)
        {
            NetworkServer.Instance.BroadcastMessage(new PlaceTileNetworkMessage(tileId, layer, posX, posY));
            SaveManager.SaveGame("C:\\Users\\Leonardo\\Documents\\Repositories\\monogame\\save\\");
        }
    }
}

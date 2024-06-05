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

        Players = worldState.Item1;
        Chunks = worldState.Item2;
        Entities = worldState.Item3;
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
            newChunk.Tiles.Add(new TileState(tileId, layer, localX, localY));
            chunk = newChunk;
            Chunks.Add(newChunk);
        }

        chunk.SetTile(tileId, layer, localX, localY);
        SaveManager.SaveGame("C:\\Users\\Leonardo\\Documents\\Repositories\\monogame\\save\\");

        NetworkServer.Instance.BroadcastMessage(new PlaceTileNetworkMessage(tileId, layer, posX, posY));
    }
}

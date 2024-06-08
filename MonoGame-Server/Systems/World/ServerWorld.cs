using Microsoft.Xna.Framework;
using MonoGame_Server.Systems.Saving;
using MonoGame_Server.Systems.Server;
using MonoGame.Source;
using MonoGame.Source.Multiplayer.Messages.World;
using MonoGame.Source.Rendering.Enum;
using MonoGame.Source.States;
using MonoGame.Source.Systems.Chunks;
using MonoGame.Source.Systems.Tiles;

namespace MonoGame_Server.Systems.World;

public class ServerWorld
{
    public List<ChunkState>? Chunks { get; set; }
    public List<PlayerState>? Players { get; set; }
    public List<EntityState>? Entities { get; set; }
    private readonly SaveManager saveManager = new();

    public void Initialize()
    {
        var worldState = this.saveManager.LoadGame();

        this.Chunks = worldState.Chunks ?? [];
        this.Players = worldState.Players ?? [];
        this.Entities = worldState.Entities ?? [];
    }

    public ChunkState? GetChunkAt(int chunkX, int chunkY)
    {
        return this.Chunks?.FirstOrDefault(x => x.X == chunkX && x.Y == chunkY);
    }

    public (List<PlayerState>? Players, List<ChunkState>? Chunks, List<EntityState>? Entities) GetWorldState()
    {
        return (this.Players, this.Chunks, this.Entities);
    }

    public PlayerState? GetPlayerByUUID(string UUID)
    {
        return this.Players?.FirstOrDefault(x => x.UUID == UUID);
    }

    public TileState? GetTileAtPosition(TileDrawLayer layer, int posX, int posY)
    {
        var chunkX = posX / Chunk.SizeX;
        var chunkY = posY / Chunk.SizeY;
        var localX = posX % Chunk.SizeX;
        var localY = posY % Chunk.SizeY;

        var chunk = this.Chunks?.FirstOrDefault(x => x.X == chunkX && x.Y == chunkY);
        return chunk?.GetTile(layer, localX, localY);
    }

    public void DestroyTileAtPosition(TileDrawLayer layer, int posX, int posY)
    {
        var chunkX = posX / Chunk.SizeX;
        var chunkY = posY / Chunk.SizeY;
        var localX = posX % Chunk.SizeX;
        var localY = posY % Chunk.SizeY;

        var chunk = this.Chunks?.FirstOrDefault(x => x.X == chunkX && x.Y == chunkY);
        var removedAny = chunk?.DestroyTile(layer, localX, localY);
        if (removedAny ?? true)
        {
            NetworkServer.Instance.BroadcastMessage(new DeleteTileNetworkMessage(layer, posX, posY));
            SaveManager.SaveGame();
        }
    }

    public void SetTileAtPosition(string tileId, TileDrawLayer layer, int posX, int posY)
    {
        var worldPosition = new Vector2(posX, posY);

        var chunkX = posX / Chunk.SizeX;
        var chunkY = posY / Chunk.SizeY;
        var localX = posX % Chunk.SizeX;
        var localY = posY % Chunk.SizeY;

        var chunk = this.Chunks?.FirstOrDefault(x => x.X == chunkX && x.Y == chunkY);

        if (chunk == null)
        {
            var newChunk = new ChunkState(chunkX, chunkY);
            chunk = newChunk;
            this.Chunks?.Add(newChunk);
        }

        var success = chunk.SetTile(tileId, layer, localX, localY);
        if (success)
        {
            NetworkServer.Instance.BroadcastMessage(new PlaceTileNetworkMessage(tileId, layer, posX, posY));
            SaveManager.SaveGame();
        }
    }

    public EntityState? GetEntityByUUID(string UUID)
    {
        return (this.Entities ?? new List<EntityState>())?.Concat(this.Players ?? new List<PlayerState>())?.FirstOrDefault(x => x.UUID == UUID);
    }

    public List<TileState> GetTilesIntersectingWithRectangle(Rectangle rectangle)
    {
        List<TileState> intersectingTiles = [];

        var chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        var chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        var startChunkX = rectangle.Left / chunkSizeInPixelsX;
        var startChunkY = rectangle.Top / chunkSizeInPixelsY;
        var endChunkX = rectangle.Right / chunkSizeInPixelsX;
        var endChunkY = rectangle.Bottom / chunkSizeInPixelsY;

        for (var chunkX = startChunkX; chunkX <= endChunkX; chunkX++)
        {
            for (var chunkY = startChunkY; chunkY <= endChunkY; chunkY++)
            {
                var chunk = this.GetChunkAt(chunkX, chunkY);
                if (chunk != null)
                {
                    var startTileX = Math.Max(0, (rectangle.Left - (chunkX * chunkSizeInPixelsX)) / Tile.PixelSizeX);
                    var startTileY = Math.Max(0, (rectangle.Top - (chunkY * chunkSizeInPixelsY)) / Tile.PixelSizeY);
                    var endTileX = Math.Min(Chunk.SizeX - 1, (rectangle.Right - (chunkX * chunkSizeInPixelsX)) / Tile.PixelSizeX);
                    var endTileY = Math.Min(Chunk.SizeY - 1, (rectangle.Bottom - (chunkY * chunkSizeInPixelsY)) / Tile.PixelSizeY);

                    for (var tileX = startTileX; tileX <= endTileX; tileX++)
                    {
                        for (var tileY = startTileY; tileY <= endTileY; tileY++)
                        {
                            var tile = chunk.GetTile(TileDrawLayer.Tiles, tileX, tileY);
                            if (tile != null)
                            {
                                intersectingTiles.Add(tile);
                            }
                        }
                    }
                }
            }
        }

        return intersectingTiles;
    }
}

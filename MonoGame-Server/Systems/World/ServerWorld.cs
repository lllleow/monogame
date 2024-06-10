using System.Drawing;
using System.Numerics;
using MonoGame_Common.Enums;
using MonoGame_Common.Messages.World;
using MonoGame_Common.States;
using MonoGame_Server.Systems.Saving;
using MonoGame_Server.Systems.Server;

namespace MonoGame_Server.Systems.World;

public class ServerWorld
{
    private readonly SaveManager saveManager = new();
    public List<ChunkState>? Chunks { get; set; }
    public List<PlayerState>? Players { get; set; }
    public List<EntityState>? Entities { get; set; }

    public void Initialize()
    {
        var worldState = saveManager.LoadGame();

        Chunks = worldState.Chunks ?? [];
        Players = worldState.Players ?? [];
        Entities = worldState.Entities ?? [];
    }

    public ChunkState? GetChunkAt(int chunkX, int chunkY)
    {
        return Chunks?.FirstOrDefault(x => x.X == chunkX && x.Y == chunkY);
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
        var chunkX = posX / ChunkState.SizeX;
        var chunkY = posY / ChunkState.SizeY;
        var localX = posX % ChunkState.SizeX;
        var localY = posY % ChunkState.SizeY;

        var chunk = Chunks?.FirstOrDefault(x => x.X == chunkX && x.Y == chunkY);
        return chunk?.GetTile(layer, localX, localY);
    }

    public void DestroyTileAtPosition(TileDrawLayer layer, int posX, int posY)
    {
        var chunkX = posX / ChunkState.SizeX;
        var chunkY = posY / ChunkState.SizeY;
        var localX = posX % ChunkState.SizeX;
        var localY = posY % ChunkState.SizeY;

        var chunk = Chunks?.FirstOrDefault(x => x.X == chunkX && x.Y == chunkY);
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

        var chunkX = posX / ChunkState.SizeX;
        var chunkY = posY / ChunkState.SizeY;
        var localX = posX % ChunkState.SizeX;
        var localY = posY % ChunkState.SizeY;

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
            SaveManager.SaveGame();
        }
    }

    public EntityState? GetEntityByUUID(string UUID)
    {
        return (Entities ?? [])?.Concat(Players ?? [])?.FirstOrDefault(x => x.UUID == UUID);
    }

    public List<TileState> GetTilesIntersectingWithRectangle(Rectangle rectangle)
    {
        List<TileState> intersectingTiles = [];

        var chunkSizeInPixelsX = ChunkState.SizeX * TileState.PixelSizeX;
        var chunkSizeInPixelsY = ChunkState.SizeY * TileState.PixelSizeY;

        var startChunkX = rectangle.Left / chunkSizeInPixelsX;
        var startChunkY = rectangle.Top / chunkSizeInPixelsY;
        var endChunkX = rectangle.Right / chunkSizeInPixelsX;
        var endChunkY = rectangle.Bottom / chunkSizeInPixelsY;

        for (var chunkX = startChunkX; chunkX <= endChunkX; chunkX++)
        for (var chunkY = startChunkY; chunkY <= endChunkY; chunkY++)
        {
            var chunk = GetChunkAt(chunkX, chunkY);
            if (chunk != null)
            {
                var startTileX = Math.Max(0, (rectangle.Left - chunkX * chunkSizeInPixelsX) / TileState.PixelSizeX);
                var startTileY = Math.Max(0, (rectangle.Top - chunkY * chunkSizeInPixelsY) / TileState.PixelSizeY);
                var endTileX = Math.Min(ChunkState.SizeX - 1,
                    (rectangle.Right - chunkX * chunkSizeInPixelsX) / TileState.PixelSizeX);
                var endTileY = Math.Min(ChunkState.SizeY - 1,
                    (rectangle.Bottom - chunkY * chunkSizeInPixelsY) / TileState.PixelSizeY);

                for (var tileX = startTileX; tileX <= endTileX; tileX++)
                for (var tileY = startTileY; tileY <= endTileY; tileY++)
                {
                    var tile = chunk.GetTile(TileDrawLayer.Tiles, tileX, tileY);
                    if (tile != null) intersectingTiles.Add(tile);
                }
            }
        }

        return intersectingTiles;
    }

    public List<TileState> GetTilesIntersectingWithMask(bool[,] mask, Rectangle rectangle)
    {
        List<TileState> intersectingTiles = [];

        // var chunkSizeInPixelsX = ChunkState.SizeX * TileState.PixelSizeX;
        // var chunkSizeInPixelsY = ChunkState.SizeY * TileState.PixelSizeY;

        // var startChunkX = rectangle.Left / chunkSizeInPixelsX;
        // var startChunkY = rectangle.Top / chunkSizeInPixelsY;
        // var endChunkX = (rectangle.Right - 1) / chunkSizeInPixelsX;
        // var endChunkY = (rectangle.Bottom - 1) / chunkSizeInPixelsY;

        // for (var chunkX = startChunkX; chunkX <= endChunkX; chunkX++)
        // {
        //     for (var chunkY = startChunkY; chunkY <= endChunkY; chunkY++)
        //     {
        //         var chunk = GetChunkAt(chunkX, chunkY);
        //         if (chunk != null)
        //         {
        //             var startTileX = Math.Max(0, (rectangle.Left - (chunkX * chunkSizeInPixelsX)) / Tile.PixelSizeX);
        //             var startTileY = Math.Max(0, (rectangle.Top - (chunkY * chunkSizeInPixelsY)) / Tile.PixelSizeY);
        //             var endTileX = Math.Min(ChunkState.SizeX - 1, (rectangle.Right - 1 - (chunkX * chunkSizeInPixelsX)) / Tile.PixelSizeX);
        //             var endTileY = Math.Min(ChunkState.SizeY - 1, (rectangle.Bottom - 1 - (chunkY * chunkSizeInPixelsY)) / Tile.PixelSizeY);

        //             for (var tileX = startTileX; tileX <= endTileX; tileX++)
        //             {
        //                 for (var tileY = startTileY; tileY <= endTileY; tileY++)
        //                 {
        //                     var tileState = chunk.GetTile(TileDrawLayer.Tiles, tileX, tileY);
        //                     ITile tile = TileRegistry.GetTile(tileState.Id);

        //                     if (tile != null)
        //                     {
        //                         if (tile.CollisionMode is CollisionMode.CollisionMask or CollisionMode.PixelPerfect)
        //                         {
        //                             var tileRect = new Rectangle(
        //                                 (chunkX * chunkSizeInPixelsX) + (tileX * Tile.PixelSizeX),
        //                                 (chunkY * chunkSizeInPixelsY) + (tileY * Tile.PixelSizeY),
        //                                 Tile.PixelSizeX,
        //                                 Tile.PixelSizeY);

        //                             var tileMask = tile.CollisionMode == CollisionMode.CollisionMask && tile.CollisionMaskSpritesheetName != null
        //                                 ? CollisionMaskHandler.GetMaskForTexture(tile.CollisionMaskSpritesheetName, tile.GetSpriteRectangle())
        //                                 : CollisionMaskHandler.GetMaskForTexture(tile.SpritesheetName, tile.GetSpriteRectangle());
        //                             var intersects = false;
        //                             for (var mx = 0; mx < mask.GetLength(0); mx++)
        //                             {
        //                                 for (var my = 0; my < mask.GetLength(1); my++)
        //                                 {
        //                                     if (mask[mx, my])
        //                                     {
        //                                         var globalMaskX = rectangle.Left + mx;
        //                                         var globalMaskY = rectangle.Top + my;

        //                                         var localTileX = globalMaskX - tileRect.Left;
        //                                         var localTileY = globalMaskY - tileRect.Top;

        //                                         if (localTileX >= 0 && localTileX < Tile.PixelSizeX && localTileY >= 0 && localTileY < Tile.PixelSizeY)
        //                                         {
        //                                             if (tileMask[localTileX, localTileY])
        //                                             {
        //                                                 intersects = true;
        //                                                 break;
        //                                             }
        //                                         }
        //                                     }
        //                                 }

        //                                 if (intersects)
        //                                 {
        //                                     break;
        //                                 }
        //                             }

        //                             if (intersects && !intersectingTiles.Contains(tileState))
        //                             {
        //                                 intersectingTiles.Add(tileState);
        //                             }
        //                         }
        //                         else
        //                         {
        //                             var tileRect = new Rectangle(
        //                                 (chunkX * chunkSizeInPixelsX) + (tileX * Tile.PixelSizeX),
        //                                 (chunkY * chunkSizeInPixelsY) + (tileY * Tile.PixelSizeY),
        //                                 Tile.PixelSizeX,
        //                                 Tile.PixelSizeY);

        //                             var intersects = false;
        //                             for (var mx = 0; mx < mask.GetLength(0); mx++)
        //                             {
        //                                 for (var my = 0; my < mask.GetLength(1); my++)
        //                                 {
        //                                     if (mask[mx, my])
        //                                     {
        //                                         var maskRect = new Rectangle(rectangle.Left + mx, rectangle.Top + my, 1, 1);
        //                                         if (tileRect.Intersects(maskRect))
        //                                         {
        //                                             intersects = true;
        //                                             break;
        //                                         }
        //                                     }
        //                                 }

        //                                 if (intersects)
        //                                 {
        //                                     break;
        //                                 }
        //                             }

        //                             if (intersects && !intersectingTiles.Contains(tileState))
        //                             {
        //                                 intersectingTiles.Add(tileState);
        //                             }
        //                         }
        //                     }
        //                 }
        //             }
        //         }
        //     }
        // }

        return intersectingTiles;
    }
}
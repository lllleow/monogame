using System.Drawing;
using System.Numerics;
using MonoGame;
using MonoGame_Common;
using MonoGame_Common.Enums;
using MonoGame_Common.Messages.World;
using MonoGame_Common.States;
using MonoGame_Common.Systems.Scripts;
using MonoGame_Common.Systems.Tiles.Interfaces;
using MonoGame_Common.Util;
using MonoGame_Common.Util.Tile;
using MonoGame_Common.Util.Tile.TileComponents;
using MonoGame_Server.Systems.Saving;
using MonoGame_Server.Systems.Server;
using MonoGame_Server.Systems.Server.Helper;

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
        }

        UpdateTextureCoordinates();
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
        {
            for (var chunkY = startChunkY; chunkY <= endChunkY; chunkY++)
            {
                var chunk = GetChunkAt(chunkX, chunkY);
                if (chunk != null)
                {
                    var startTileX = Math.Max(0, (rectangle.Left - (chunkX * chunkSizeInPixelsX)) / TileState.PixelSizeX);
                    var startTileY = Math.Max(0, (rectangle.Top - (chunkY * chunkSizeInPixelsY)) / TileState.PixelSizeY);
                    var endTileX = Math.Min(
                        ChunkState.SizeX - 1,
                        (rectangle.Right - (chunkX * chunkSizeInPixelsX)) / TileState.PixelSizeX);
                    var endTileY = Math.Min(
                        ChunkState.SizeY - 1,
                        (rectangle.Bottom - (chunkY * chunkSizeInPixelsY)) / TileState.PixelSizeY);

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

    public List<TileState> GetTilesIntersectingWithMask(bool[,] mask, Rectangle rectangle)
    {
        List<TileState> intersectingTiles = new List<TileState>();
        List<TileState> tilesIntersectingRectangle = GetTilesIntersectingWithRectangle(rectangle);

        foreach (TileState tileState in tilesIntersectingRectangle)
        {
            CommonTile? tile = TileRegistry.GetTile(tileState.Id);
            if (tile == null || (tile.CollisionMode != CollisionMode.CollisionMask && tile.CollisionMode != CollisionMode.PixelPerfect)) continue;

            TextureRendererTileComponent tileComponent = tile.GetComponent<TextureRendererTileComponent>();

            bool[,] tileMask = tile.CollisionMode == CollisionMode.CollisionMask && tile.CollisionMaskSpritesheetName != null
                ? ServerTextureHelper.GetImageMaskForRectangle(tile.CollisionMaskSpritesheetName, tileComponent.GetSpriteRectangle())
                : ServerTextureHelper.GetImageMaskForRectangle(tile.SpritesheetName, tileComponent.GetSpriteRectangle());

            Rectangle tileRect = new Rectangle(((tileState.ChunkX ?? 0) * SharedGlobals.PixelSizeX) + ((tileState.LocalX ?? 0) * SharedGlobals.PixelSizeX), ((tileState.ChunkY ?? 0) * SharedGlobals.PixelSizeY) + ((tileState.LocalY ?? 0) * SharedGlobals.PixelSizeY), tile.TileSizeX * SharedGlobals.PixelSizeX, tile.TileSizeY * SharedGlobals.PixelSizeY);

            NetworkServer.Instance.BroadcastMessage(new RenderMaskNetworkMessage(tileRect, tileMask));
            if (IsMaskIntersecting(tileMask, tileRect, mask, rectangle))
            {
                intersectingTiles.Add(tileState);
            }
        }

        return intersectingTiles;
    }

    private bool IsMaskIntersecting(bool[,] tileMask, Rectangle tileRect, bool[,] globalMask, Rectangle globalRect)
    {
        int startX = Math.Max(tileRect.Left, globalRect.Left);
        int endX = Math.Min(tileRect.Right, globalRect.Right);
        int startY = Math.Max(tileRect.Top, globalRect.Top);
        int endY = Math.Min(tileRect.Bottom, globalRect.Bottom);

        for (int y = startY; y < endY; y++)
        {
            for (int x = startX; x < endX; x++)
            {
                int tileX = x - tileRect.Left;
                int tileY = y - tileRect.Top;
                int globalX = x - globalRect.Left;
                int globalY = y - globalRect.Top;

                if (tileX >= 0 && tileX < tileMask.GetLength(1) &&
                    tileY >= 0 && tileY < tileMask.GetLength(0) &&
                    globalX >= 0 && globalX < globalMask.GetLength(1) &&
                    globalY >= 0 && globalY < globalMask.GetLength(0))
                {
                    if (tileMask[tileY, tileX] && globalMask[globalY, globalX])
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void UpdateTextureCoordinates()
    {
        foreach (var chunk in Chunks)
        {
            foreach (var tileState in chunk.Tiles)
            {
                var tile = TileRegistry.GetTile(tileState.Id);
                if (tile?.HasComponent<TextureRendererTileComponent>() ?? false)
                {
                    TextureRendererTileComponent textureRendererTileComponent = tile.GetComponent<TextureRendererTileComponent>();
                    var worldPosition = chunk.GetWorldPosition(tileState.LocalX ?? 0, tileState.LocalY ?? 0);
                    TileNeighborConfiguration tileNeighborConfiguration = TileServerHelper.GetNeighborConfiguration(tile, tileState.Layer, (int)worldPosition.X, (int)worldPosition.Y);
                    textureRendererTileComponent.UpdateTextureCoordinates(tileNeighborConfiguration, tileState.Layer);
                }
            }
        }
    }
}
using System.Drawing;
using System.Numerics;
using MonoGame;
using MonoGame_Common;
using MonoGame_Common.Enums;
using MonoGame_Common.Messages.World;
using MonoGame_Common.States;
using MonoGame_Common.States.TileComponents;
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
        // var worldState = saveManager.LoadGame();

        // Chunks = worldState.Chunks ?? [];
        // Players = worldState.Players ?? [];
        // Entities = worldState.Entities ?? [];

        Chunks = [];
        Players = [];
        Entities = [];

        ChunkState newChunk = new ChunkState();
        newChunk.SetTile("base.wall", TileDrawLayer.Tiles, 8, 8);
        Chunks.Add(newChunk);
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

    public List<PositionedTileHelper> GetTilesIntersectingWithRectangle(Rectangle rectangle)
    {
        List<PositionedTileHelper> intersectingTiles = [];

        var chunkSizeInPixelsX = ChunkState.SizeX * SharedGlobals.PixelSizeX;
        var chunkSizeInPixelsY = ChunkState.SizeY * SharedGlobals.PixelSizeY;

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
                    var startTileX = Math.Max(0, (rectangle.Left - (chunkX * chunkSizeInPixelsX)) / SharedGlobals.PixelSizeX);
                    var startTileY = Math.Max(0, (rectangle.Top - (chunkY * chunkSizeInPixelsY)) / SharedGlobals.PixelSizeY);
                    var endTileX = Math.Min(
                        ChunkState.SizeX - 1,
                        (rectangle.Right - (chunkX * chunkSizeInPixelsX)) / SharedGlobals.PixelSizeX);
                    var endTileY = Math.Min(
                        ChunkState.SizeY - 1,
                        (rectangle.Bottom - (chunkY * chunkSizeInPixelsY)) / SharedGlobals.PixelSizeY);

                    for (var tileX = startTileX; tileX <= endTileX; tileX++)
                    {
                        for (var tileY = startTileY; tileY <= endTileY; tileY++)
                        {
                            var tile = chunk.GetTile(TileDrawLayer.Tiles, tileX, tileY);
                            PositionedTileHelper positionedTile = new PositionedTileHelper(tile, chunk, tileX, tileY);
                            if (tile != null)
                            {
                                intersectingTiles.Add(positionedTile);
                            }
                        }
                    }
                }
            }
        }

        return intersectingTiles;
    }

    public List<PositionedTileHelper> GetTilesIntersectingWithMask(bool[,] mask, Rectangle rectangle)
    {
        List<PositionedTileHelper> intersectingTiles = new List<PositionedTileHelper>();
        List<PositionedTileHelper> tilesIntersectingRectangle = GetTilesIntersectingWithRectangle(rectangle);

        foreach (PositionedTileHelper positionedTile in tilesIntersectingRectangle)
        {
            CommonTile? commonTile = positionedTile.Tile.GetCommonTile();
            if (commonTile == null || (commonTile.CollisionMode != CollisionMode.CollisionMask && commonTile.CollisionMode != CollisionMode.PixelPerfect)) continue;

            TextureRendererTileComponentState tileComponent = positionedTile.Tile.GetComponent<TextureRendererTileComponentState>();

            bool[,] tileMask = commonTile.CollisionMode == CollisionMode.CollisionMask && commonTile.CollisionMaskSpritesheetName != null
                ? ServerTextureHelper.GetImageMaskForRectangle(commonTile.CollisionMaskSpritesheetName, tileComponent.GetSpriteRectangle())
                : ServerTextureHelper.GetImageMaskForRectangle(commonTile.SpritesheetName, tileComponent.GetSpriteRectangle());

            Rectangle tileRect = positionedTile.GetTileRect();
            NetworkServer.Instance.BroadcastMessage(new RenderMaskNetworkMessage(tileRect, tileMask));

            if (IsMaskIntersecting(tileMask, tileRect, mask, rectangle))
            {
                intersectingTiles.Add(positionedTile);
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
            foreach (var layer in chunk.Tiles.Keys)
            {
                foreach (var tileState in chunk.Tiles[layer])
                {
                    if (tileState?.HasComponent<TextureRendererTileComponentState>() ?? false)
                    {
                        TextureRendererTileComponentState textureRendererTileComponent = tileState.GetComponent<TextureRendererTileComponentState>();
                        var worldPosition = chunk.GetWorldPosition(chunk.GetTilePosition(tileState));
                        TileNeighborConfiguration tileNeighborConfiguration = TileServerHelper.GetNeighborConfiguration(tileState.GetCommonTile(), layer, (int)worldPosition.X, (int)worldPosition.Y);
                        textureRendererTileComponent.UpdateTextureCoordinates(tileNeighborConfiguration);
                    }
                }
            }
        }
    }
}
﻿using System.Drawing;
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
        var worldState = SaveManager.LoadGame();

        Chunks = worldState.Chunks ?? [];
        Players = worldState.Players ?? [];
        Entities = worldState.Entities ?? [];

        UpdateTextureCoordinates();
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
            NetworkServer.Instance.BroadcastMessage(new PlaceTileNetworkMessage()
            {
                TileId = tileId,
                Layer = layer,
                PosX = posX,
                PosY = posY
            });
            UpdateTextureCoordinates();
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
                            if (tile != null)
                            {
                                PositionedTileHelper positionedTile = new PositionedTileHelper(tile, chunk, tileX, tileY);
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
        Dictionary<string, PositionedTileHelper> intersectingTiles = new Dictionary<string, PositionedTileHelper>();
        List<PositionedTileHelper> tiles = GetTilesIntersectingWithRectangle(rectangle);
        foreach (var positionedTile in tiles)
        {
            TileState? tile = positionedTile.Tile;
            CommonTile? commonTile = tile?.GetCommonTile();
            if (commonTile == null) continue;

            if (tile != null && !commonTile.Walkable)
            {
                PositionedTileHelper positionedTileHelper = new(tile, positionedTile.Chunk, positionedTile.PosX, positionedTile.PosY);
                Rectangle tileRect = positionedTileHelper.GetTileRect();

                TextureRendererTileComponentState? tileComponent = tile.GetComponent<TextureRendererTileComponentState>();
                if (tileComponent == null) continue;

                bool[,] tileMask;

                if (commonTile.CollisionMode == CollisionMode.CollisionMask && commonTile.CollisionMaskSpritesheetName != null)
                {
                    tileMask = ServerTextureHelper.GetImageMaskForRectangle(commonTile.CollisionMaskSpritesheetName, tileComponent.GetSpriteRectangle());
                }
                else
                {
                    if (commonTile.SpritesheetName == null) continue;
                    tileMask = ServerTextureHelper.GetImageMaskForRectangle(commonTile.SpritesheetName, tileComponent.GetSpriteRectangle());
                }

                // NetworkServer.Instance.BroadcastMessage(new RenderMaskNetworkMessage()
                // {
                //     Rectangle = tileRect,
                //     Mask = tileMask
                // });

                if (tile.Id != null && CollisionMaskHandler.CheckMaskCollision(tileMask, rectangle, tileMask, tileRect))
                {
                    intersectingTiles.TryAdd(tile.Id, positionedTileHelper);
                }
            }
        }
        return intersectingTiles.Values.ToList();
    }

    public void UpdateTextureCoordinates()
    {
        foreach (var chunk in Chunks ?? [])
        {
            foreach (var layer in chunk.Tiles.Keys)
            {
                foreach (var tileState in chunk.Tiles[layer])
                {
                    if (tileState?.HasComponent<TextureRendererTileComponentState>() ?? false)
                    {
                        TextureRendererTileComponentState? textureRendererTileComponent = tileState.GetComponent<TextureRendererTileComponentState>();
                        var worldPosition = chunk.GetWorldPosition(chunk.GetTilePosition(tileState));
                        CommonTile? commonTile = tileState.GetCommonTile();
                        if (commonTile == null) continue;
                        TileNeighborConfiguration tileNeighborConfiguration = TileServerHelper.GetNeighborConfiguration(commonTile, layer, (int)worldPosition.X, (int)worldPosition.Y);
                        textureRendererTileComponent?.UpdateTextureCoordinates(tileNeighborConfiguration);
                    }
                }
            }
        }
    }
}
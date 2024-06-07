using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Multiplayer.Messages.Player;
using MonoGame.Source.Multiplayer.Messages.World;
using MonoGame.Source.Rendering.Enum;
using MonoGame.Source.Systems.Chunks;
using MonoGame.Source.Systems.Chunks.Interfaces;
using MonoGame.Source.Systems.Components.Collision;
using MonoGame.Source.Systems.Components.Collision.Enum;
using MonoGame.Source.Systems.Entity.Interfaces;
using MonoGame.Source.Systems.Entity.PlayerNamespace;
using MonoGame.Source.Systems.Tiles;
using MonoGame.Source.Systems.Tiles.Interfaces;
using MonoGame.Source.WorldNamespace.WorldStates;

namespace MonoGame.Source.WorldNamespace;

public class World
{
    private List<IGameEntity> Entities { get; set; } = [];
    public List<Player> Players { get; set; } = [];
    private List<IChunk> Chunks { get; set; } = [];

    public World()
    {
        ClientNetworkEventManager.Subscribe<ChunkDataNetworkMessage>(message =>
        {
            LoadChunkFromChunkState(message.ChunkState);
        });

        ClientNetworkEventManager.Subscribe<DeleteTileNetworkMessage>(message =>
        {
            DeleteTile(TileDrawLayer.Tiles, message.PosX, message.PosY);
        });

        ClientNetworkEventManager.Subscribe<PlaceTileNetworkMessage>(message =>
        {
            SetTileAtPosition(message.TileId, TileDrawLayer.Tiles, message.PosX, message.PosY);
        });

        ClientNetworkEventManager.Subscribe<SpawnPlayerNetworkMessage>(message =>
        {
            var player = new Player(message.UUID, message.Position);
            Players.Add(player);
        });
    }

    public void LoadChunkFromChunkState(ChunkState chunkState)
    {
        IChunk chunk = new Chunk(chunkState);
        Chunks.Add(chunk);
        chunk.UpdateNeighborChunks();
    }

    public void Update(GameTime gameTime)
    {
        foreach (var entity in GetEntities())
        {
            entity.Update(gameTime);
        }
    }

    public void UpdateAllTextureCoordinates()
    {
        foreach (var chunk in Chunks)
        {
            chunk.UpdateTextureCoordinates();
        }
    }

    public List<IGameEntity> GetEntities()
    {
        return Players.Cast<IGameEntity>().Concat(Entities).ToList();
    }

    public IChunk GetChunkAt(int x, int y)
    {
        return Chunks.Find(c => c.X == x && c.Y == y);
    }

    public IChunk CreateOrGetChunk(int x, int y)
    {
        var existingChunk = GetChunkAt(x, y);
        if (existingChunk == null)
        {
            var chunk = new Chunk(this, x, y);
            Chunks.Add(chunk);
            return chunk;
        }
        else
        {
            return existingChunk;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        DrawWorld();
        foreach (var entity in GetEntities())
        {
            entity.Draw(spriteBatch);
        }
    }

    public Player GetPlayerByUUID(string id)
    {
        return Players.Find(p => p.UUID == id);
    }

    public Player GetLocalPlayer()
    {
        return Players.Where(p => p.UUID == Globals.UUID).FirstOrDefault();
    }

    public ITile GetTileAtPosition(Vector2 worldPosition)
    {
        var globalX = (int)(worldPosition.X / Chunk.SizeX);
        var globalY = (int)(worldPosition.Y / Chunk.SizeY);
        return GetTileAt(0, globalX, globalY);
    }

    public ITile SetTileAtPosition(string tile, TileDrawLayer layer, int globalX, int globalY)
    {
        var localPosition = GetLocalPositionFromGlobal(globalX, globalY);
        var chunkPosition = GetChunkPositionFromGlobal(globalX, globalY);

        var chunk = Globals.World.CreateOrGetChunk(chunkPosition.ChunkPositionX, chunkPosition.ChunkPositionY);
        return chunk.SetTileAndUpdateNeighbors(tile, layer, localPosition.LocalX, localPosition.LocalY);
    }

    public ITile GetTileAt(TileDrawLayer layer, int globalX, int globalY)
    {
        var localPosition = GetLocalPositionFromGlobal(globalX, globalY);
        var chunk = GetChunkFromGlobalPosition(globalX, globalY);

        return chunk?.GetTile(layer: layer, x: localPosition.LocalX, y: localPosition.LocalY);
    }

    public List<ITile> GetAllTilesFromLayerAt(int globalX, int globalY)
    {
        var chunkX = globalX / Chunk.SizeX;
        var chunkY = globalY / Chunk.SizeY;
        var tileX = globalX % Chunk.SizeX;
        var tileY = globalY % Chunk.SizeY;
        var chunk = Chunks.Find(c => c.X == chunkX && c.Y == chunkY);

        if (chunk != null)
        {
            List<ITile> tiles = [];
            foreach (var layer in chunk.Tiles.Keys)
            {
                var tile = chunk.GetTile(layer, tileX, tileY);
                tiles.Add(tile);
            }

            return tiles;
        }
        else
        {
            return null;
        }
    }

    public (int LocalX, int LocalY) GetLocalPositionFromGlobal(int globalPositionX, int globalPositionY)
    {
        var localX = globalPositionX % Chunk.SizeX;
        var localY = globalPositionY % Chunk.SizeY;

        return (localX, localY);
    }

    public (int ChunkPositionX, int ChunkPositionY) GetChunkPositionFromGlobal(int globalPositionX, int globalPositionY)
    {
        var chunkX = globalPositionX / Chunk.SizeX;
        var chunkY = globalPositionY / Chunk.SizeY;

        return (chunkX, chunkY);
    }

    public IChunk GetChunkFromGlobalPosition(int globalPositionX, int globalPositionY)
    {
        var chunkPosition = GetChunkPositionFromGlobal(globalPositionX, globalPositionY);
        return GetChunkAt(chunkPosition.ChunkPositionX, chunkPosition.ChunkPositionY);
    }

    public ITile GetTileAtScreenPosition(TileDrawLayer layer, int screenX, int screenY)
    {
        var worldPosition = new Vector2(screenX, screenY);
        worldPosition = Vector2.Transform(worldPosition, Matrix.Invert(Globals.Camera.Transform));

        var chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        var chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        var chunkX = (int)(worldPosition.X / chunkSizeInPixelsX);
        var chunkY = (int)(worldPosition.Y / chunkSizeInPixelsY);

        var localX = (int)(worldPosition.X % chunkSizeInPixelsX) / Tile.PixelSizeX;
        var localY = (int)(worldPosition.Y % chunkSizeInPixelsY) / Tile.PixelSizeY;

        var chunk = Globals.World.GetChunkAt(chunkX, chunkY);
        return chunk?.GetTile(layer, localX, localY) ?? null;
    }

    public List<ITile> GetTilesIntersectingWithMask(bool[,] mask, Rectangle rectangle)
    {
        List<ITile> intersectingTiles = [];

        var chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        var chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        var startChunkX = rectangle.Left / chunkSizeInPixelsX;
        var startChunkY = rectangle.Top / chunkSizeInPixelsY;
        var endChunkX = (rectangle.Right - 1) / chunkSizeInPixelsX;
        var endChunkY = (rectangle.Bottom - 1) / chunkSizeInPixelsY;

        for (var chunkX = startChunkX; chunkX <= endChunkX; chunkX++)
        {
            for (var chunkY = startChunkY; chunkY <= endChunkY; chunkY++)
            {
                var chunk = GetChunkAt(chunkX, chunkY);
                if (chunk != null)
                {
                    var startTileX = Math.Max(0, (rectangle.Left - (chunkX * chunkSizeInPixelsX)) / Tile.PixelSizeX);
                    var startTileY = Math.Max(0, (rectangle.Top - (chunkY * chunkSizeInPixelsY)) / Tile.PixelSizeY);
                    var endTileX = Math.Min(Chunk.SizeX - 1, (rectangle.Right - 1 - (chunkX * chunkSizeInPixelsX)) / Tile.PixelSizeX);
                    var endTileY = Math.Min(Chunk.SizeY - 1, (rectangle.Bottom - 1 - (chunkY * chunkSizeInPixelsY)) / Tile.PixelSizeY);

                    for (var tileX = startTileX; tileX <= endTileX; tileX++)
                    {
                        for (var tileY = startTileY; tileY <= endTileY; tileY++)
                        {
                            var tile = chunk.GetTile(TileDrawLayer.Tiles, tileX, tileY);
                            if (tile != null)
                            {
                                if (tile.CollisionMode is CollisionMode.CollisionMask or CollisionMode.PixelPerfect)
                                {
                                    var tileRect = new Rectangle(
                                        (chunkX * chunkSizeInPixelsX) + (tileX * Tile.PixelSizeX),
                                        (chunkY * chunkSizeInPixelsY) + (tileY * Tile.PixelSizeY),
                                        Tile.PixelSizeX,
                                        Tile.PixelSizeY);

                                    var tileMask = tile.CollisionMode == CollisionMode.CollisionMask && tile.CollisionMaskSpritesheetName != null
                                        ? CollisionMaskHandler.GetMaskForTexture(tile.CollisionMaskSpritesheetName, tile.GetSpriteRectangle())
                                        : CollisionMaskHandler.GetMaskForTexture(tile.SpritesheetName, tile.GetSpriteRectangle());
                                    var intersects = false;
                                    for (var mx = 0; mx < mask.GetLength(0); mx++)
                                    {
                                        for (var my = 0; my < mask.GetLength(1); my++)
                                        {
                                            if (mask[mx, my])
                                            {
                                                var globalMaskX = rectangle.Left + mx;
                                                var globalMaskY = rectangle.Top + my;

                                                var localTileX = globalMaskX - tileRect.Left;
                                                var localTileY = globalMaskY - tileRect.Top;

                                                if (localTileX >= 0 && localTileX < Tile.PixelSizeX && localTileY >= 0 && localTileY < Tile.PixelSizeY)
                                                {
                                                    if (tileMask[localTileX, localTileY])
                                                    {
                                                        intersects = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }

                                        if (intersects)
                                        {
                                            break;
                                        }
                                    }

                                    if (intersects && !intersectingTiles.Contains(tile))
                                    {
                                        intersectingTiles.Add(tile);
                                    }
                                }
                                else
                                {
                                    var tileRect = new Rectangle(
                                        (chunkX * chunkSizeInPixelsX) + (tileX * Tile.PixelSizeX),
                                        (chunkY * chunkSizeInPixelsY) + (tileY * Tile.PixelSizeY),
                                        Tile.PixelSizeX,
                                        Tile.PixelSizeY);

                                    var intersects = false;
                                    for (var mx = 0; mx < mask.GetLength(0); mx++)
                                    {
                                        for (var my = 0; my < mask.GetLength(1); my++)
                                        {
                                            if (mask[mx, my])
                                            {
                                                var maskRect = new Rectangle(rectangle.Left + mx, rectangle.Top + my, 1, 1);
                                                if (tileRect.Intersects(maskRect))
                                                {
                                                    intersects = true;
                                                    break;
                                                }
                                            }
                                        }

                                        if (intersects)
                                        {
                                            break;
                                        }
                                    }

                                    if (intersects && !intersectingTiles.Contains(tile))
                                    {
                                        intersectingTiles.Add(tile);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return intersectingTiles;
    }

    public bool Intersects(Rectangle rectA, Rectangle rectB)
    {
        return rectA.X < rectB.X + rectB.Width &&
               rectA.X + rectA.Width > rectB.X &&
               rectA.Y < rectB.Y + rectB.Height &&
               rectA.Y + rectA.Height > rectB.Y;
    }

    public List<ITile> GetTilesIntersectingWithRectangle(Rectangle rectangle)
    {
        List<ITile> intersectingTiles = [];

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
                var chunk = GetChunkAt(chunkX, chunkY);
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
                            var tile = chunk.GetTile(TileDrawLayer.Terrain, tileX, tileY);
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

    public List<ITile> GetTilesIntersectingWithCircle(Vector2 position, float radius)
    {
        List<ITile> intersectingTiles = [];

        var chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        var chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        var startChunkX = (int)((position.X - radius) / chunkSizeInPixelsX);
        var startChunkY = (int)((position.Y - radius) / chunkSizeInPixelsY);
        var endChunkX = (int)((position.X + radius) / chunkSizeInPixelsX);
        var endChunkY = (int)((position.Y + radius) / chunkSizeInPixelsY);

        for (var chunkX = startChunkX; chunkX <= endChunkX; chunkX++)
        {
            for (var chunkY = startChunkY; chunkY <= endChunkY; chunkY++)
            {
                var chunk = GetChunkAt(chunkX, chunkY);
                if (chunk != null)
                {
                    var chunkStartX = chunkX * Chunk.SizeX;
                    var chunkStartY = chunkY * Chunk.SizeY;
                    var chunkEndX = chunkStartX + Chunk.SizeX;
                    var chunkEndY = chunkStartY + Chunk.SizeY;

                    for (var tileX = chunkStartX; tileX < chunkEndX; tileX++)
                    {
                        for (var tileY = chunkStartY; tileY < chunkEndY; tileY++)
                        {
                            var tilePosition = new Vector2(tileX * Tile.PixelSizeX, tileY * Tile.PixelSizeY);
                            if (Vector2.Distance(tilePosition, position) <= radius)
                            {
                                foreach (var layer in chunk.Tiles.Keys)
                                {
                                    var tile = chunk.GetTile(layer, tileX - chunkStartX, tileY - chunkStartY);
                                    intersectingTiles.Add(tile);
                                }
                            }
                        }
                    }
                }
            }
        }

        return intersectingTiles;
    }

    public IChunk GetChunkAtScreenPosition(int layer, int screenX, int screenY)
    {
        var chunkPosition = GetChunkFromScreenPosition(new Vector2(screenX, screenY));
        var chunk = Globals.World.GetChunkAt(chunkPosition.ChunkPositionX, chunkPosition.ChunkPositionY);
        return chunk;
    }

    public (int PosX, int PosY) GetGlobalPositionFromScreenPosition(Vector2 screenPositionBeforeTransform)
    {
        var screenPosition = Vector2.Transform(screenPositionBeforeTransform, Matrix.Invert(Globals.Camera.Transform));

        var chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        var chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        var chunkX = (int)(screenPosition.X / chunkSizeInPixelsX);
        var chunkY = (int)(screenPosition.Y / chunkSizeInPixelsY);

        var localX = (int)(screenPosition.X % chunkSizeInPixelsX) / Tile.PixelSizeX;
        var localY = (int)(screenPosition.Y % chunkSizeInPixelsY) / Tile.PixelSizeY;

        return ((chunkX * Chunk.SizeX) + localX, (chunkY * Chunk.SizeY) + localY);
    }

    public (int ChunkPositionX, int ChunkPositionY) GetChunkFromScreenPosition(Vector2 screenPositionBeforeTransform)
    {
        var screenPosition = Vector2.Transform(screenPositionBeforeTransform, Matrix.Invert(Globals.Camera.Transform));

        var chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        var chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        var chunkX = (int)(screenPosition.X / chunkSizeInPixelsX);
        var chunkY = (int)(screenPosition.Y / chunkSizeInPixelsY);

        return (chunkX, chunkY);
    }

    private void DrawWorld()
    {
        foreach (var chunk in Chunks)
        {
            chunk.Draw(Globals.SpriteBatch);
        }
    }

    internal void DeleteTile(TileDrawLayer layer, int posX, int posY)
    {
        var chunk = GetChunkFromGlobalPosition(posX, posY);
        var localPosition = GetLocalPositionFromGlobal(posX, posY);
        chunk.DeleteTile(layer, localPosition.LocalX, localPosition.LocalY);
    }
}

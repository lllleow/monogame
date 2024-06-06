using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Chunks;
using MonoGame.Source.Systems.Chunks.Interfaces;
using MonoGame.Source.Systems.Components.Collision;

namespace MonoGame;

public class World
{


    private List<IGameEntity> Entities { get; set; } = new List<IGameEntity>();
    public List<Player> Players { get; set; } = new List<Player>();
    private List<IChunk> Chunks { get; set; } = new List<IChunk>();

    public World()
    {
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
        foreach (IChunk chunk in Chunks)
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
        IChunk existingChunk = GetChunkAt(x, y);
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
        int globalX = (int)(worldPosition.X / Chunk.SizeX);
        int globalY = (int)(worldPosition.Y / Chunk.SizeY);
        return GetTileAt(0, globalX, globalY);
    }

    public ITile SetTileAtPosition(string tile, TileDrawLayer layer, int globalX, int globalY)
    {
        (int, int) localPosition = GetLocalPositionFromGlobal(globalX, globalY);
        (int, int) chunkPosition = GetChunkPositionFromGlobal(globalX, globalY);

        IChunk chunk = Globals.World.CreateOrGetChunk(chunkPosition.Item1, chunkPosition.Item2);
        return chunk.SetTileAndUpdateNeighbors(tile, layer, localPosition.Item1, localPosition.Item2);
    }

    public ITile GetTileAt(TileDrawLayer layer, int globalX, int globalY)
    {
        (int, int) localPosition = GetLocalPositionFromGlobal(globalX, globalY);
        IChunk chunk = GetChunkFromGlobalPosition(globalX, globalY);

        if (chunk != null)
        {
            return chunk.GetTile(layer: layer, x: localPosition.Item1, y: localPosition.Item2);
        }
        else
        {
            return null;
        }
    }

    public List<ITile> GetAllTilesFromLayerAt(int globalX, int globalY)
    {
        int chunkX = globalX / Chunk.SizeX;
        int chunkY = globalY / Chunk.SizeY;
        int tileX = globalX % Chunk.SizeX;
        int tileY = globalY % Chunk.SizeY;
        var chunk = Chunks.Find(c => c.X == chunkX && c.Y == chunkY);

        if (chunk != null)
        {
            List<ITile> tiles = new List<ITile>();
            foreach (TileDrawLayer layer in chunk.Tiles.Keys)
            {
                ITile tile = chunk.GetTile(layer, tileX, tileY);
                tiles.Add(tile);
            }
            return tiles;
        }
        else
        {
            return null;
        }
    }

    public (int, int) GetLocalPositionFromGlobal(int posX, int posY)
    {
        int localX = posX % Chunk.SizeX;
        int localY = posY % Chunk.SizeY;

        return (localX, localY);
    }

    public (int, int) GetChunkPositionFromGlobal(int posX, int posY)
    {
        int chunkX = posX / Chunk.SizeX;
        int chunkY = posY / Chunk.SizeY;

        return (chunkX, chunkY);
    }

    public IChunk GetChunkFromGlobalPosition(int posX, int posY)
    {
        (int, int) chunkPosition = GetChunkPositionFromGlobal(posX, posY);
        return GetChunkAt(chunkPosition.Item1, chunkPosition.Item2);
    }

    public ITile GetTileAtScreenPosition(TileDrawLayer layer, int screenX, int screenY)
    {
        Vector2 worldPosition = new Vector2(screenX, screenY);
        worldPosition = Vector2.Transform(worldPosition, Matrix.Invert(Globals.Camera.Transform));

        int chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        int chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        int chunkX = (int)(worldPosition.X / chunkSizeInPixelsX);
        int chunkY = (int)(worldPosition.Y / chunkSizeInPixelsY);

        int localX = (int)(worldPosition.X % chunkSizeInPixelsX) / Tile.PixelSizeX;
        int localY = (int)(worldPosition.Y % chunkSizeInPixelsY) / Tile.PixelSizeY;

        IChunk chunk = Globals.World.GetChunkAt(chunkX, chunkY);
        return chunk?.GetTile(layer, localX, localY) ?? null;
    }

    public List<ITile> GetTilesIntersectingWithMask(bool[,] mask, Rectangle rectangle)
    {
        List<ITile> intersectingTiles = new List<ITile>();

        int chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        int chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        int startChunkX = rectangle.Left / chunkSizeInPixelsX;
        int startChunkY = rectangle.Top / chunkSizeInPixelsY;
        int endChunkX = (rectangle.Right - 1) / chunkSizeInPixelsX;
        int endChunkY = (rectangle.Bottom - 1) / chunkSizeInPixelsY;

        for (int chunkX = startChunkX; chunkX <= endChunkX; chunkX++)
        {
            for (int chunkY = startChunkY; chunkY <= endChunkY; chunkY++)
            {
                IChunk chunk = GetChunkAt(chunkX, chunkY);
                if (chunk != null)
                {
                    int startTileX = Math.Max(0, (rectangle.Left - chunkX * chunkSizeInPixelsX) / Tile.PixelSizeX);
                    int startTileY = Math.Max(0, (rectangle.Top - chunkY * chunkSizeInPixelsY) / Tile.PixelSizeY);
                    int endTileX = Math.Min(Chunk.SizeX - 1, (rectangle.Right - 1 - chunkX * chunkSizeInPixelsX) / Tile.PixelSizeX);
                    int endTileY = Math.Min(Chunk.SizeY - 1, (rectangle.Bottom - 1 - chunkY * chunkSizeInPixelsY) / Tile.PixelSizeY);

                    for (int tileX = startTileX; tileX <= endTileX; tileX++)
                    {
                        for (int tileY = startTileY; tileY <= endTileY; tileY++)
                        {
                            ITile tile = chunk.GetTile(TileDrawLayer.Tiles, tileX, tileY);
                            if (tile != null)
                            {
                                if (tile.CollisionMode == CollisionMode.CollisionMask || tile.CollisionMode == CollisionMode.PixelPerfect)
                                {
                                    Rectangle tileRect = new Rectangle(chunkX * chunkSizeInPixelsX + tileX * Tile.PixelSizeX,
                                                                       chunkY * chunkSizeInPixelsY + tileY * Tile.PixelSizeY,
                                                                       Tile.PixelSizeX, Tile.PixelSizeY);

                                    bool[,] tileMask;
                                    if (tile.CollisionMode == CollisionMode.CollisionMask && tile.CollisionMaskSpritesheetName != null)
                                    {
                                        tileMask = CollisionMaskHandler.GetMaskForTexture(tile.CollisionMaskSpritesheetName, tile.GetSpriteRectangle());
                                    }
                                    else
                                    {
                                        tileMask = CollisionMaskHandler.GetMaskForTexture(tile.SpritesheetName, tile.GetSpriteRectangle());
                                    }

                                    bool intersects = false;
                                    for (int mx = 0; mx < mask.GetLength(0); mx++)
                                    {
                                        for (int my = 0; my < mask.GetLength(1); my++)
                                        {
                                            if (mask[mx, my])
                                            {
                                                int globalMaskX = rectangle.Left + mx;
                                                int globalMaskY = rectangle.Top + my;

                                                int localTileX = globalMaskX - tileRect.Left;
                                                int localTileY = globalMaskY - tileRect.Top;

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
                                        if (intersects) break;
                                    }

                                    if (intersects && !intersectingTiles.Contains(tile))
                                    {
                                        intersectingTiles.Add(tile);
                                    }
                                }
                                else
                                {
                                    Rectangle tileRect = new Rectangle(chunkX * chunkSizeInPixelsX + tileX * Tile.PixelSizeX,
                                                                   chunkY * chunkSizeInPixelsY + tileY * Tile.PixelSizeY,
                                                                   Tile.PixelSizeX, Tile.PixelSizeY);

                                    bool intersects = false;
                                    for (int mx = 0; mx < mask.GetLength(0); mx++)
                                    {
                                        for (int my = 0; my < mask.GetLength(1); my++)
                                        {
                                            if (mask[mx, my])
                                            {
                                                Rectangle maskRect = new Rectangle(rectangle.Left + mx, rectangle.Top + my, 1, 1);
                                                if (tileRect.Intersects(maskRect))
                                                {
                                                    intersects = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (intersects) break;
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
        List<ITile> intersectingTiles = new List<ITile>();

        int chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        int chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        int startChunkX = rectangle.Left / chunkSizeInPixelsX;
        int startChunkY = rectangle.Top / chunkSizeInPixelsY;
        int endChunkX = rectangle.Right / chunkSizeInPixelsX;
        int endChunkY = rectangle.Bottom / chunkSizeInPixelsY;

        for (int chunkX = startChunkX; chunkX <= endChunkX; chunkX++)
        {
            for (int chunkY = startChunkY; chunkY <= endChunkY; chunkY++)
            {
                IChunk chunk = GetChunkAt(chunkX, chunkY);
                if (chunk != null)
                {
                    int startTileX = Math.Max(0, (rectangle.Left - chunkX * chunkSizeInPixelsX) / Tile.PixelSizeX);
                    int startTileY = Math.Max(0, (rectangle.Top - chunkY * chunkSizeInPixelsY) / Tile.PixelSizeY);
                    int endTileX = Math.Min(Chunk.SizeX - 1, (rectangle.Right - chunkX * chunkSizeInPixelsX) / Tile.PixelSizeX);
                    int endTileY = Math.Min(Chunk.SizeY - 1, (rectangle.Bottom - chunkY * chunkSizeInPixelsY) / Tile.PixelSizeY);

                    for (int tileX = startTileX; tileX <= endTileX; tileX++)
                    {
                        for (int tileY = startTileY; tileY <= endTileY; tileY++)
                        {
                            ITile tile = chunk.GetTile(TileDrawLayer.Terrain, tileX, tileY);
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
        List<ITile> intersectingTiles = new List<ITile>();

        int chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        int chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        int startChunkX = (int)((position.X - radius) / chunkSizeInPixelsX);
        int startChunkY = (int)((position.Y - radius) / chunkSizeInPixelsY);
        int endChunkX = (int)((position.X + radius) / chunkSizeInPixelsX);
        int endChunkY = (int)((position.Y + radius) / chunkSizeInPixelsY);

        for (int chunkX = startChunkX; chunkX <= endChunkX; chunkX++)
        {
            for (int chunkY = startChunkY; chunkY <= endChunkY; chunkY++)
            {
                IChunk chunk = GetChunkAt(chunkX, chunkY);
                if (chunk != null)
                {
                    int chunkStartX = chunkX * Chunk.SizeX;
                    int chunkStartY = chunkY * Chunk.SizeY;
                    int chunkEndX = chunkStartX + Chunk.SizeX;
                    int chunkEndY = chunkStartY + Chunk.SizeY;

                    for (int tileX = chunkStartX; tileX < chunkEndX; tileX++)
                    {
                        for (int tileY = chunkStartY; tileY < chunkEndY; tileY++)
                        {
                            Vector2 tilePosition = new Vector2(tileX * Tile.PixelSizeX, tileY * Tile.PixelSizeY);
                            if (Vector2.Distance(tilePosition, position) <= radius)
                            {
                                foreach (TileDrawLayer layer in chunk.Tiles.Keys)
                                {
                                    ITile tile = chunk.GetTile(layer, tileX - chunkStartX, tileY - chunkStartY);
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
        (int, int) chunkPosition = GetChunkFromScreenPosition(new Vector2(screenX, screenY));
        IChunk chunk = Globals.World.GetChunkAt(chunkPosition.Item1, chunkPosition.Item2);
        return chunk;
    }

    public (int, int) GetGlobalPositionFromScreenPosition(Vector2 screenPositionBeforeTransform)
    {
        Vector2 screenPosition = Vector2.Transform(screenPositionBeforeTransform, Matrix.Invert(Globals.Camera.Transform));

        int chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        int chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        int chunkX = (int)(screenPosition.X / chunkSizeInPixelsX);
        int chunkY = (int)(screenPosition.Y / chunkSizeInPixelsY);

        int localX = (int)(screenPosition.X % chunkSizeInPixelsX) / Tile.PixelSizeX;
        int localY = (int)(screenPosition.Y % chunkSizeInPixelsY) / Tile.PixelSizeY;

        return (chunkX * Chunk.SizeX + localX, chunkY * Chunk.SizeY + localY);
    }

    public (int, int) GetChunkFromScreenPosition(Vector2 screenPositionBeforeTransform)
    {
        Vector2 screenPosition = Vector2.Transform(screenPositionBeforeTransform, Matrix.Invert(Globals.Camera.Transform));

        int chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        int chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        int chunkX = (int)(screenPosition.X / chunkSizeInPixelsX);
        int chunkY = (int)(screenPosition.Y / chunkSizeInPixelsY);

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
        IChunk chunk = GetChunkFromGlobalPosition(posX, posY);
        (int, int) localPosition = GetLocalPositionFromGlobal(posX, posY);
        chunk.DeleteTile(layer, localPosition.Item1, localPosition.Item2);
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class World
{
    private List<IGameEntity> Entities { get; set; } = new List<IGameEntity>();
    private List<IChunk> Chunks { get; set; } = new List<IChunk>();
    public Player Player;

    public void InitWorld()
    {
        Player = new Player(new Vector2(0, 0), 32, 32);
        Entities.Add(Player);

        InitializeChunks();
    }

    public void Update(GameTime gameTime)
    {
        foreach (var entity in Entities)
        {
            entity.Update(gameTime);
        }
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
            chunk.Generate();
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
        foreach (var entity in Entities)
        {
            entity.Draw(spriteBatch);
        }
    }

    public ITile GetTileAtPosition(Vector2 worldPosition)
    {
        int globalX = (int)(worldPosition.X / Chunk.SizeX);
        int globalY = (int)(worldPosition.Y / Chunk.SizeY);
        return GetTileAt(0, globalX, globalY);
    }

    public ITile SetTileAtPosition(string tile, int layer, int globalX, int globalY)
    {
        int chunkX = globalX / Chunk.SizeX;
        int chunkY = globalY / Chunk.SizeY;
        int tileX = globalX % Chunk.SizeX;
        int tileY = globalY % Chunk.SizeY;
        var chunk = Chunks.Find(c => c.X == chunkX && c.Y == chunkY);

        if (chunk == null)
        {
            var newChunk = new Chunk(this, tileX, tileY);
            newChunk.Generate();
            Chunks.Add(newChunk);
            chunk = newChunk;
        }

        return chunk.SetTile(tile, layer, tileX, tileY);
    }

    public ITile GetTileAt(int layer, int globalX, int globalY)
    {
        int chunkX = globalX / Chunk.SizeX;
        int chunkY = globalY / Chunk.SizeY;
        int tileX = globalX % Chunk.SizeX;
        int tileY = globalY % Chunk.SizeY;
        var chunk = Chunks.Find(c => c.X == chunkX && c.Y == chunkY);

        if (chunk != null)
        {
            return chunk.GetTile(layer: layer, x: tileX, y: tileY);
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
            foreach (int layer in chunk.Tiles.Keys)
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

    public ITile GetTileAtScreenPosition(int layer, int screenX, int screenY)
    {
        Vector2 worldPosition = new Vector2(screenX, screenY);
        worldPosition = Vector2.Transform(worldPosition, Matrix.Invert(Globals.camera.Transform));

        int chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        int chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        int chunkX = (int)(worldPosition.X / chunkSizeInPixelsX);
        int chunkY = (int)(worldPosition.Y / chunkSizeInPixelsY);

        int localX = (int)(worldPosition.X % chunkSizeInPixelsX) / Tile.PixelSizeX;
        int localY = (int)(worldPosition.Y % chunkSizeInPixelsY) / Tile.PixelSizeY;

        IChunk chunk = Globals.world.GetChunkAt(chunkX, chunkY);
        return chunk?.GetTile(layer, localX, localY) ?? null;
    }

    public List<ITile> GetTilesIntersecting(Rectangle rectangle)
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
                IChunk chunk = Globals.world.GetChunkAt(chunkX, chunkY);
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
                            ITile tile = chunk.GetTile(layer: 2, x: tileX, y: tileY);
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

    public IChunk GetChunkAtScreenPosition(int layer, int screenX, int screenY)
    {
        Vector2 worldPosition = new Vector2(screenX, screenY);
        worldPosition = Vector2.Transform(worldPosition, Matrix.Invert(Globals.camera.Transform));

        int chunkSizeInPixelsX = Chunk.SizeX * Tile.PixelSizeX;
        int chunkSizeInPixelsY = Chunk.SizeY * Tile.PixelSizeY;

        int chunkX = (int)(worldPosition.X / chunkSizeInPixelsX);
        int chunkY = (int)(worldPosition.Y / chunkSizeInPixelsY);

        int localX = (int)(worldPosition.X % chunkSizeInPixelsX) / Tile.PixelSizeX;
        int localY = (int)(worldPosition.Y % chunkSizeInPixelsY) / Tile.PixelSizeY;

        IChunk chunk = Globals.world.GetChunkAt(chunkX, chunkY);
        return chunk;
    }

    private void InitializeChunks()
    {
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                var chunk = new Chunk(this, x, y);
                chunk.Generate();
                Chunks.Add(chunk);
            }
        }

        foreach (IChunk chunk in Chunks)
        {
            chunk.UpdateTextureCoordinates();
        }

        Player.Teleport(new Vector2(1000, 1000));
    }

    private void DrawWorld()
    {
        foreach (var chunk in Chunks)
        {
            chunk.Draw(Globals.spriteBatch);
        }
    }
}

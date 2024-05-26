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

    private void InitializeChunks()
    {
        for (int x = 0; x < 1; x++)
        {
            for (int y = 0; y < 1; y++)
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
    }

    private void DrawWorld()
    {
        foreach (var chunk in Chunks)
        {
            chunk.Draw(Globals.spriteBatch);
        }
    }
}

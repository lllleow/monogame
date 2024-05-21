using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class World
{
    public List<IGameEntity> Entities { get; set; } = new List<IGameEntity>();
    public List<IChunk> Chunks { get; set; } = new List<IChunk>();

    public void InitWorld()
    {
        var player = new Player("textures/player", new Vector2(100, 100), new Vector2(50, 50));
        Entities.Add(player);

        InitializeChunks();
    }

    public void Update(GameTime gameTime)
    {
        foreach (var entity in Entities)
        {
            entity.BaseUpdate(gameTime);
            entity.Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        DrawWorld();
        foreach (var entity in Entities)
        {
            if (entity is IDrawable)
            {
                (entity as IDrawable).Draw(spriteBatch);
                (entity as IDrawable).BaseDraw(spriteBatch);
            }
        }
    }

    public void InitializeChunks()
    {
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 16; y++)
            {
                var chunk = new Chunk(x, y);
                chunk.Initialize();
                Chunks.Add(chunk);
            }
        }
    }

    const int TILE_SIZE_X = 32;
    const int TILE_SIZE_Y = 32;
    public void DrawWorld()
    {
        foreach (var chunk in Chunks)
        {
            for (int chunkX = 0; chunkX < Chunk.SizeX; chunkX++)
            {
                for (int chunkY = 0; chunkY < Chunk.SizeY; chunkY++)
                {
                    var tile = chunk.GetTile(chunkX, chunkY);
                    if (tile != null)
                    {
                        int x = (chunk.X * Chunk.SizeX * TILE_SIZE_X) + (chunkX * tile.SizeX * TILE_SIZE_X);
                        int y = (chunk.Y * Chunk.SizeY * TILE_SIZE_Y) + (chunkY * tile.SizeY * TILE_SIZE_Y);
                        Globals.spriteBatch.Draw(tile.Texture, new Vector2(x, y), Color.White);
                    }
                }
            }
        }
    }
}

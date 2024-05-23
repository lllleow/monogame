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

    public void Draw(SpriteBatch spriteBatch)
    {
        DrawWorld();
        foreach (var entity in Entities)
        {
            entity.Draw(spriteBatch);
        }
    }

    public ITile GetTileAt(int globalX, int globalY)
    {
        int chunkX = globalX / Chunk.SizeX;
        int chunkY = globalY / Chunk.SizeY;
        int tileX = globalX % Chunk.SizeX;
        int tileY = globalY % Chunk.SizeY;
        var chunk = Chunks.Find(c => c.X == chunkX && c.Y == chunkY);

        if (chunk != null)
        {
            return chunk.GetTile(tileX, tileY);
        }
        else
        {
            return null;
        }
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
    }

    private void DrawWorld()
    {
        foreach (var chunk in Chunks)
        {
            chunk.Draw(Globals.spriteBatch);
        }
    }
}

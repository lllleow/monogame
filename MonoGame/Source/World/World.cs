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

    public void DrawWorld()
    {
        foreach (var chunk in Chunks)
        {
            chunk.Draw(Globals.spriteBatch);
        }
    }
}

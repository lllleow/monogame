using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class Chunk : IChunk
{
    public ITile[,] Tiles { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public static int SizeX { get; set; } = 16;
    public static int SizeY { get; set; } = 16;

    public Chunk(int X, int Y)
    {
        this.X = X;
        this.Y = Y;
    }

    public ITile GetTile(int x, int y)
    {
        return Tiles[x, y];
    }

    public void Initialize()
    {
        Tiles = new ITile[SizeX, SizeY];
        for (int chunkX = 0; chunkX < SizeX; chunkX++)
        {
            for (int chunkY = 0; chunkY < SizeY; chunkY++)
            {
                SetTile("base.stone", chunkX, chunkY);
            }
        }
    }

    public ITile SetTile(string id, int x, int y)
    {
        ITile tile = TileRegistry.GetTile(id);
        tile.Initialize();
        Tiles[x, y] = tile;
        return tile;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int chunkX = 0; chunkX < SizeX; chunkX++)
        {
            for (int chunkY = 0; chunkY < SizeY; chunkY++)
            {
                var tile = GetTile(chunkX, chunkY);
                if (tile != null)
                {
                    int x = (X * SizeX * Tile.PixelSizeX) + (chunkX * tile.SizeX * Tile.PixelSizeX);
                    int y = (Y * SizeY * Tile.PixelSizeY) + (chunkY * tile.SizeY * Tile.PixelSizeY);
                    spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(tile.SpritesheetName), new Vector2(x, y), tile.GetSpriteRectangle(), Color.White);
                }
            }
        }
    }
}

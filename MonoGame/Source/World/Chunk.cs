using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DotnetNoise;

namespace MonoGame;

public class Chunk : IChunk
{
    public ITile[,] Tiles { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public static int SizeX { get; set; } = 16;
    public static int SizeY { get; set; } = 16;
    private World World;

    public Chunk(World world, int X, int Y)
    {
        this.X = X;
        this.Y = Y;
        this.World = world;
    }

    public ITile GetTile(int x, int y)
    {
        return Tiles[x, y];
    }

    public float offsetX = 100f;
    public float offsetY = 100f;
    public float scale = 20f;
    public void Initialize()
    {
        Tiles = new ITile[SizeX, SizeY];

        FastNoise fastNoise = new FastNoise();

        for (int chunkX = 0; chunkX < SizeX; chunkX++)
        {
            for (int chunkY = 0; chunkY < SizeY; chunkY++)
            {
                Vector2 worldPosition = GetWorldPosition(chunkX, chunkY);
                float xCoord = offsetX + worldPosition.X / SizeX * scale;
                float yCoord = offsetY + worldPosition.Y / SizeY * scale;
                double sample = fastNoise.GetNoise(xCoord * 5, yCoord * 5, 0);

                if (sample < -0.1)
                    SetTile("base.water", chunkX, chunkY);
                else if (sample < 0.2)
                    SetTile("base.stone", chunkX, chunkY);
                else
                    SetTile("base.grass", chunkX, chunkY);
            }
        }
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        int worldX = (X * SizeX) + x;
        int worldY = (Y * SizeY) + y;
        return new Vector2(worldX, worldY);
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

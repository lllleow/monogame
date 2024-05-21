using System;

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
                if (chunkY == 0 || chunkX == 0)
                {
                    SetTile("base.stone", chunkX, chunkY);
                }
                else
                {
                    SetTile("base.grass", chunkX, chunkY);
                }
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
}

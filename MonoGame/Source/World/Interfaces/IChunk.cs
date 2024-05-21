using System;

namespace MonoGame;

public interface IChunk : IInitializable
{
    public ITile[,] Tiles { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public ITile GetTile(int x, int y);
    public ITile SetTile(string id, int x, int y);
}

using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public interface IChunk
{
    public ITile[,] Tiles { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public ITile GetTile(int x, int y);
    public ITile SetTile(string id, int x, int y);
    public void UpdateTextureCoordinates();
    public void Draw(SpriteBatch spriteBatch);
    void UpdateNeighborTiles();
}

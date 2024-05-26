using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public interface IChunk
{
    public Dictionary<int, ITile[,]> Tiles { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public ITile GetTile(int layer, int x, int y);
    public void DeleteTile(int layer, int x, int y);
    public void UpdateNeighborChunks();
    public ITile SetTile(string id, int layer, int x, int y);
    public ITile SetTileAndUpdateNeighbors(string id, int layer, int x, int y);
    public void UpdateTextureCoordinates();
    public void Draw(SpriteBatch spriteBatch);
    void UpdateNeighborTiles();
}

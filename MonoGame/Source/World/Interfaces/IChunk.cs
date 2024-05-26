﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public interface IChunk
{
    public Dictionary<TileDrawLayer, ITile[,]> Tiles { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public ITile GetTile(TileDrawLayer layer, int x, int y);
    public void DeleteTile(TileDrawLayer layer, int x, int y);
    public void UpdateNeighborChunks();
    public ITile SetTile(string id, TileDrawLayer layer, int x, int y);
    public ITile SetTileAndUpdateNeighbors(string id, TileDrawLayer layer, int x, int y);
    public void UpdateTextureCoordinates();
    public void Draw(SpriteBatch spriteBatch);
    void UpdateNeighborTiles(TileDrawLayer layer);
}

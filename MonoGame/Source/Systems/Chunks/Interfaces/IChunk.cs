using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Common.Enums;
using MonoGame.Source.Systems.Tiles.Interfaces;

namespace MonoGame.Source.Systems.Chunks.Interfaces;

public interface IChunk
{
    public Dictionary<TileDrawLayer, ITile[,]> Tiles { get; set; }

    public int X { get; set; }

    public int Y { get; set; }

    public void UpdateNeighborTiles(TileDrawLayer layer);

    public void DeleteTile(TileDrawLayer layer, int x, int y);

    public void UpdateNeighborChunks();

    public void UpdateTextureCoordinates();

    public void Draw(SpriteBatch spriteBatch);

    public ITile GetTile(TileDrawLayer layer, int x, int y);

    public ITile SetTile(string id, TileDrawLayer layer, int x, int y);

    public ITile SetTileAndUpdateNeighbors(string id, TileDrawLayer layer, int x, int y);
}

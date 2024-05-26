using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Source.Systems.Chunks.Interfaces;

/// <summary>
/// Represents a chunk in a game world.
/// </summary>
public interface IChunk
{
    /// <summary>
    /// Gets or sets the tiles in the chunk, organized by draw layer.
    /// </summary>
    public Dictionary<TileDrawLayer, ITile[,]> Tiles { get; set; }

    /// <summary>
    /// Gets or sets the X coordinate of the chunk.
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate of the chunk.
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// Updates the neighbor tiles of a specific draw layer.
    /// </summary>
    /// <param name="layer">The draw layer to update.</param>
    public void UpdateNeighborTiles(TileDrawLayer layer);

    /// <summary>
    /// Deletes a tile at the specified coordinates and draw layer.
    /// </summary>
    /// <param name="layer">The draw layer of the tile.</param>
    /// <param name="x">The X coordinate of the tile.</param>
    /// <param name="y">The Y coordinate of the tile.</param>
    public void DeleteTile(TileDrawLayer layer, int x, int y);

    /// <summary>
    /// Updates the neighbor chunks of the current chunk.
    /// </summary>
    public void UpdateNeighborChunks();

    /// <summary>
    /// Updates the texture coordinates of the tiles in the chunk.
    /// </summary>
    public void UpdateTextureCoordinates();

    /// <summary>
    /// Draws the chunk using the specified sprite batch.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch to use for drawing.</param>
    public void Draw(SpriteBatch spriteBatch);

    /// <summary>
    /// Gets the tile at the specified coordinates and draw layer.
    /// </summary>
    /// <param name="layer">The draw layer of the tile.</param>
    /// <param name="x">The X coordinate of the tile.</param>
    /// <param name="y">The Y coordinate of the tile.</param>
    /// <returns>The tile at the specified coordinates and draw layer.</returns>
    public ITile GetTile(TileDrawLayer layer, int x, int y);

    /// <summary>
    /// Sets the tile at the specified coordinates and draw layer.
    /// </summary>
    /// <param name="id">The ID of the tile to set.</param>
    /// <param name="layer">The draw layer of the tile.</param>
    /// <param name="x">The X coordinate of the tile.</param>
    /// <param name="y">The Y coordinate of the tile.</param>
    /// <returns>The newly set tile.</returns>
    public ITile SetTile(string id, TileDrawLayer layer, int x, int y);

    /// <summary>
    /// Sets the tile at the specified coordinates and draw layer, and updates the neighbor tiles.
    /// </summary>
    /// <param name="id">The ID of the tile to set.</param>
    /// <param name="layer">The draw layer of the tile.</param>
    /// <param name="x">The X coordinate of the tile.</param>
    /// <param name="y">The Y coordinate of the tile.</param>
    /// <returns>The newly set tile.</returns>
    public ITile SetTileAndUpdateNeighbors(string id, TileDrawLayer layer, int x, int y);
}

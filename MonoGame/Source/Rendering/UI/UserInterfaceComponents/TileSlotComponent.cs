using System;
using Microsoft.Xna.Framework;

namespace MonoGame;

/// <summary>
/// Represents a slot component that can hold a tile.
/// </summary>
public class TileSlotComponent : SlotComponent
{
    public ITile Tile;

    public TileSlotComponent(string name, ITile tile, Vector2 localPosition) : base(name, localPosition)
    {
        Tile = tile;
    }

    /// <summary>
    /// Sets the tile for this slot component.
    /// </summary>
    /// <param name="tile">The tile to set.</param>
    public void SetTile(ITile tile)
    {
        Tile = tile;
    }

    /// <summary>
    /// Gets the texture location of the tile.
    /// </summary>
    /// <returns>The texture location of the tile.</returns>
    public override TextureLocation GetDrawable()
    {
        return Tile?.GetTextureLocation();
    }
}

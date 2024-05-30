﻿using System;
using Microsoft.Xna.Framework;

namespace MonoGame;

/// <summary>
/// Represents a slot component that can hold a tile.
/// </summary>
public class TileSlotComponent : SlotComponent
{
    ITile tile;

    public TileSlotComponent(string name, Vector2 position, Vector2 size) : base(name, position, size)
    {
        
    }

    /// <summary>
    /// Sets the tile for this slot component.
    /// </summary>
    /// <param name="tile">The tile to set.</param>
    public void SetTile(ITile tile)
    {
        this.tile = tile;
    }

    /// <summary>
    /// Gets the texture location of the tile.
    /// </summary>
    /// <returns>The texture location of the tile.</returns>
    public override TextureLocation GetDrawable()
    {
        return tile?.GetTextureLocation();
    }
}

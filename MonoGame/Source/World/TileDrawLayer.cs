using System;

namespace MonoGame;

public enum TileDrawLayer
{
    Background,
    Terrain,
    Tiles
}

public static class TileDrawLayerPriority
{
    public static TileDrawLayer[] GetPriority()
    {
        return new TileDrawLayer[]
        {
            TileDrawLayer.Background,
            TileDrawLayer.Terrain,
            TileDrawLayer.Tiles
        };
    }
}

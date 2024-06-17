namespace MonoGame_Common.Enums;

public enum TileDrawLayer
{
    Background,

    Terrain,

    Tiles,
    Walls
}

public static class TileDrawLayerPriority
{
    public static TileDrawLayer[] GetPriority()
    {
        return new[]
        {
            TileDrawLayer.Background,
            TileDrawLayer.Terrain,
            TileDrawLayer.Tiles,
            TileDrawLayer.Walls
        };
    }
}
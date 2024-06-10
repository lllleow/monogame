using MonoGame_Common.Enums;

namespace MonoGame.Source.Systems.Tiles.Interfaces;

public abstract class ITile
{
    public string Id { get; set; } = "base.abstract.tile";

    public string Name { get; set; } = "Abstract Tile";

    public string SpritesheetName { get; set; }

    public int TileSizeX { get; set; } = 1;

    public int TileSizeY { get; set; } = 1;

    public ITileTextureProcessor TextureProcessor { get; set; }

    public CollisionMode CollisionMode { get; set; } = CollisionMode.BoundingBox;

    public string CollisionMaskSpritesheetName { get; set; } = null;

    public bool Walkable { get; set; } = false;

    public (int TextureCoordinateX, int TextureCoordinateY) DefaultTextureCoordinates { get; set; } = (0, 0);

    public string[] ConnectableTiles { get; set; } = [];
}
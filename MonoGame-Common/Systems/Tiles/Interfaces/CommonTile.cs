using MonoGame_Common.Enums;
using MonoGame_Common.Util.Tile;
using MonoGame_Common.Util.Tile.TileComponents;

namespace MonoGame_Common.Systems.Tiles.Interfaces;

public class CommonTile
{
    public string Id { get; set; } = "base.abstract.tile";

    public string Name { get; set; } = "Abstract Tile";

    public string? SpritesheetName { get; set; }

    public int TileSizeX { get; set; } = 1;

    public int TileSizeY { get; set; } = 1;

    public CollisionMode CollisionMode { get; set; } = CollisionMode.BoundingBox;

    public string? CollisionMaskSpritesheetName { get; set; } = null;

    public bool Walkable { get; set; } = false;

    public (int TextureCoordinateX, int TextureCoordinateY) DefaultTextureCoordinates { get; set; } = (0, 0);

    public string[] ConnectableTiles { get; set; } = [];

    public List<ITileComponent> Components { get; set; } = [];
    public ITileTextureProcessor? TextureProcessor { get; set; }

    public CommonTile()
    {
        Components.Add(new TextureRendererTileComponent(DefaultTextureCoordinates.TextureCoordinateX, DefaultTextureCoordinates.TextureCoordinateY, this));
    }

    public void AddComponent(ITileComponent component)
    {
        Components.Add(component);
    }

    public void RemoveComponent<T>()
    where T : ITileComponent
    {
        _ = Components.RemoveAll(component => component is T);
    }

    public T? GetComponent<T>()
    where T : ITileComponent
    {
        return Components.OfType<T>().FirstOrDefault();
    }

    public bool HasComponent<T>()
    where T : ITileComponent
    {
        return Components.Any(component => component is T);
    }
}

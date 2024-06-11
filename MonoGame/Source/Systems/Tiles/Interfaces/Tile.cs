using System.Collections.Generic;
using System.Linq;
using MonoGame_Common.Enums;

namespace MonoGame.Source.Systems.Tiles.Interfaces;

public abstract class Tile
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

    public List<ITileComponent> Components { get; set; } = new();

    public Tile()
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
        Components.RemoveAll(component => component is T);
    }

    public T GetComponent<T>()
    where T : ITileComponent
    {
        return (T)Components.FirstOrDefault(component => component is T);
    }

    public bool HasComponent<T>()
    where T : ITileComponent
    {
        return Components.Any(component => component is T);
    }
}
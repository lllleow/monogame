using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Rendering.Enum;
using MonoGame.Source.Systems.Components.Collision.Enum;
using MonoGame.Source.Systems.Tiles.Enums;
using MonoGame.Source.Systems.Tiles.Interfaces;
using MonoGame.Source.Systems.Tiles.Utils;
using MonoGame.Source.Util.Enum;
namespace MonoGame.Source.Systems.Tiles;

public class Tile : ITile
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string SpritesheetName { get; set; }

    public int TextureX { get; set; }

    public int TextureY { get; set; }

    public (int TextureCoordinateX, int TextureCoordinateY) CurrentTextureIndex { get; set; }

    public Texture2D Texture { get; set; }

    public int SizeX { get; set; } = 1;

    public int SizeY { get; set; } = 1;

    public float Scale { get; set; } = 1;

    public float Opacity { get; set; } = 1;

    public static int PixelSizeX { get; set; } = 16;

    public static int PixelSizeY { get; set; } = 16;

    public int PixelOffsetX { get; set; }

    public int PixelOffsetY { get; set; }

    public ITileTextureProcessor TextureProcessor { get; set; }

    public CollisionMode CollisionMode { get; set; } = CollisionMode.BoundingBox;

    public string CollisionMaskSpritesheetName { get; set; }

    public int WorldX { get; set; }

    public int WorldY { get; set; }

    public bool Walkable { get; set; } = true;

    public static bool ShowTileBoundingBox { get; set; } = false;

    public string[] ConnectableTiles { get; set; } = System.Array.Empty<string>();

    public (int TextureCoordinateX, int TextureCoordinateY) DefaultTextureCoordinates { get; set; }
    public int LocalX { get; set; }
    public int LocalY { get; set; }

    public Tile()
    {
    }

    public void Initialize(int localX, int localY, int worldX, int worldY)
    {
        WorldX = worldX;
        WorldY = worldY;
        LocalX = localX;
        LocalY = localY;
    }

    public void UpdateTextureCoordinates(TileDrawLayer layer)
    {
        var configuration = GetNeighborConfiguration(layer);
        var coordinates = TextureProcessor?.Process(configuration) ?? (0, 0);

        CurrentTextureIndex = coordinates;

        TextureX = coordinates.TextureCoordinateX;
        TextureY = coordinates.TextureCoordinateY;
    }

    public TileNeighborConfiguration GetNeighborConfiguration(TileDrawLayer layer)
    {
        var left = Globals.World.GetTileAt(layer, WorldX - 1, WorldY);
        var right = Globals.World.GetTileAt(layer, WorldX + 1, WorldY);
        var up = Globals.World.GetTileAt(layer, WorldX, WorldY - 1);
        var down = Globals.World.GetTileAt(layer, WorldX, WorldY + 1);

        var left_top = Globals.World.GetTileAt(layer, WorldX - 1, WorldY - 1);
        var right_top = Globals.World.GetTileAt(layer, WorldX + 1, WorldY - 1);
        var left_bottom = Globals.World.GetTileAt(layer, WorldX - 1, WorldY + 1);
        var right_bottom = Globals.World.GetTileAt(layer, WorldX + 1, WorldY + 1);

        return new TileNeighborConfiguration(this, left, right, up, down, left_top, right_top, left_bottom, right_bottom);
    }

    public void OnNeighborChanged(ITile neighbor, TileDrawLayer layer, Direction direction)
    {
        if (TextureProcessor != null)
        {
            UpdateTextureCoordinates(layer);
        }
    }
}

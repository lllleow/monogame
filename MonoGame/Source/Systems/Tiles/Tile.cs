using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Tiles.Utils;
namespace MonoGame;

public class Tile : ITile
{

    public string Id { get; set; }

    public string Name { get; set; }

    public string SpritesheetName { get; set; }

    public int TextureX { get; set; }

    public int TextureY { get; set; }

    public (int, int) CurrentTextureIndex { get; set; }

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

    public static bool ShowTileBoundingBox = false;

    public string[] ConnectableTiles { get; set; } = System.Array.Empty<string>();

    public (int, int) DefaultTextureCoordinates { get; set; }
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
        TileNeighborConfiguration configuration = GetNeighborConfiguration(layer);
        (int, int) coordinates = TextureProcessor?.Process(configuration) ?? (0, 0);

        CurrentTextureIndex = coordinates;

        TextureX = coordinates.Item1;
        TextureY = coordinates.Item2;
    }

    public TileNeighborConfiguration GetNeighborConfiguration(TileDrawLayer layer)
    {
        ITile left = Globals.World.GetTileAt(layer, WorldX - 1, WorldY);
        ITile right = Globals.World.GetTileAt(layer, WorldX + 1, WorldY);
        ITile up = Globals.World.GetTileAt(layer, WorldX, WorldY - 1);
        ITile down = Globals.World.GetTileAt(layer, WorldX, WorldY + 1);

        ITile left_top = Globals.World.GetTileAt(layer, WorldX - 1, WorldY - 1);
        ITile right_top = Globals.World.GetTileAt(layer, WorldX + 1, WorldY - 1);
        ITile left_bottom = Globals.World.GetTileAt(layer, WorldX - 1, WorldY + 1);
        ITile right_bottom = Globals.World.GetTileAt(layer, WorldX + 1, WorldY + 1);

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

using MonoGame.Source;
using MonoGame_Common.Enums;
using MonoGame_Common.Systems.Tiles.Interfaces;

namespace MonoGame_Common.Util.Tile;

public class TileClientHelper
{
    public static TileNeighborConfiguration GetNeighborConfiguration(CommonTile centerTile, TileDrawLayer layer, int worldX, int worldY)
    {
        var left = Globals.World.GetTileAt(layer, worldX - 1, worldY);
        var right = Globals.World.GetTileAt(layer, worldX + 1, worldY);
        var up = Globals.World.GetTileAt(layer, worldX, worldY - 1);
        var down = Globals.World.GetTileAt(layer, worldX, worldY + 1);

        var left_top = Globals.World.GetTileAt(layer, worldX - 1, worldY - 1);
        var right_top = Globals.World.GetTileAt(layer, worldX + 1, worldY - 1);
        var left_bottom = Globals.World.GetTileAt(layer, worldX - 1, worldY + 1);
        var right_bottom = Globals.World.GetTileAt(layer, worldX + 1, worldY + 1);

        return new TileNeighborConfiguration(centerTile, left, right, up, down, left_top, right_top, left_bottom, right_bottom);
    }
}

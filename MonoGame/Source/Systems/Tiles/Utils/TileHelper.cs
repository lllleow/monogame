using System;
using MonoGame.Source;
using MonoGame.Source.Systems.Tiles.Interfaces;
using MonoGame.Source.Systems.Tiles.Utils;
using MonoGame_Common.Enums;

namespace MonoGame;

public class TileHelper
{
    public static TileNeighborConfiguration GetNeighborConfiguration(Tile centerTile, TileDrawLayer layer, int worldX, int worldY)
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

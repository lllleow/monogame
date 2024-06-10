using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.Utils;
using MonoGame.Source.Systems.Tiles.Interfaces;
using MonoGame.Source.Systems.Tiles.Utils;
using MonoGame_Common.Enums;
using MonoGame_Common.Util.Enum;

namespace MonoGame.Source.Systems.Tiles;

public class PlacedTile
{
    public ITile Tile { get; set; }
    public int WorldX { get; set; }
    public int WorldY { get; set; }
    public int LocalX { get; set; }
    public int LocalY { get; set; }
    public int TextureX { get; set; }
    public int TextureY { get; set; }

    public PlacedTile(ITile tile, int worldX, int worldY)
    {
        Tile = tile;
        WorldX = worldX;
        WorldY = worldY;
    }

    public TextureLocation GetTextureLocation()
    {
        return new TextureLocation(Tile.SpritesheetName, GetSpriteRectangle());
    }

    public Rectangle GetSpriteRectangle()
    {
        return new Rectangle(TextureX * Globals.PixelSizeX, TextureY * Globals.PixelSizeY, Tile.TileSizeX * Globals.PixelSizeX, Tile.TileSizeY * Globals.PixelSizeY);
    }

    public void UpdateTextureCoordinates(TileDrawLayer layer)
    {
        var configuration = GetNeighborConfiguration(layer);
        var (TextureCoordinateX, TextureCoordinateY) = Tile.TextureProcessor?.Process(configuration) ?? (0, 0);
        TextureX = TextureCoordinateX;
        TextureY = TextureCoordinateY;
    }

    public void OnNeighborChanged(PlacedTile neighbor, TileDrawLayer layer, Direction direction)
    {
        if (Tile.TextureProcessor != null)
        {
            UpdateTextureCoordinates(layer);
        }
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
}

using System.Drawing;
using MonoGame_Common;
using MonoGame_Common.Enums;
using MonoGame_Common.Systems.Tiles.Interfaces;
using MonoGame_Common.Util.Helpers;
using MonoGame_Common.Util.Tile;
using MonoGame_Common.Util.Tile.TileComponents;

namespace MonoGame.Source.Systems.Tiles.TileComponents;

public class TextureRendererTileComponent : ITileComponent
{
    public int TextureX { get; set; }
    public int TextureY { get; set; }
    public CommonTile Tile { get; set; }

    public TextureRendererTileComponent(int textureX, int textureY, CommonTile tile)
    {
        TextureX = textureX;
        TextureY = textureY;
        Tile = tile;
    }

    public TextureLocation GetTextureLocation()
    {
        return new TextureLocation(Tile.SpritesheetName, GetSpriteRectangle());
    }

    public Rectangle GetSpriteRectangle()
    {
        return new Rectangle(TextureX * SharedGlobals.PixelSizeX, TextureY * SharedGlobals.PixelSizeY, Tile.TileSizeX * SharedGlobals.PixelSizeX, Tile.TileSizeY * SharedGlobals.PixelSizeY);
    }

    public void UpdateTextureCoordinates(TileNeighborConfiguration configuration, TileDrawLayer layer)
    {
        (int, int) coordinates = Tile.TextureProcessor?.Process(configuration) ?? (0, 0);
        TextureX = coordinates.Item1;
        TextureY = coordinates.Item2;
    }
}

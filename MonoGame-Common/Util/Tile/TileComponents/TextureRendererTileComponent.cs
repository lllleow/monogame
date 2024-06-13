using System.Drawing;
using MonoGame_Common.Enums;
using MonoGame_Common.States.TileComponents;
using MonoGame_Common.Systems.Tiles.Interfaces;
using MonoGame_Common.Util.Helpers;

namespace MonoGame_Common.Util.Tile.TileComponents;

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

    public void UpdateTextureCoordinates(TileNeighborConfiguration configuration)
    {
        (int, int) coordinates = Tile.TextureProcessor?.Process(configuration) ?? (0, 0);
        TextureX = coordinates.Item1;
        TextureY = coordinates.Item2;
    }

    public TileComponentState GetTileComponentState()
    {
        TextureRendererTileComponentState tileComponentState = new TextureRendererTileComponentState();
        tileComponentState.TextureX = TextureX;
        tileComponentState.TextureY = TextureY;
        return tileComponentState;
    }
}

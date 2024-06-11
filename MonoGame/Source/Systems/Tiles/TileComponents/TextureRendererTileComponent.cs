using Microsoft.Xna.Framework;
using MonoGame_Common.Enums;
using MonoGame_Common.Util.Enum;
using MonoGame.Source;
using MonoGame.Source.Rendering.Utils;
using MonoGame.Source.Systems.Tiles.Interfaces;

namespace MonoGame;

public class TextureRendererTileComponent : ITileComponent
{
    public int TextureX { get; set; }
    public int TextureY { get; set; }
    public Tile Tile { get; set; }

    public TextureRendererTileComponent(int textureX, int textureY, Tile tile)
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
        return new Rectangle(TextureX * Globals.PixelSizeX, TextureY * Globals.PixelSizeY, Tile.TileSizeX * Globals.PixelSizeX, Tile.TileSizeY * Globals.PixelSizeY);
    }

    public void UpdateTextureCoordinates(int worldX, int worldY, TileDrawLayer layer)
    {
        var configuration = TileHelper.GetNeighborConfiguration(Tile, layer, worldX, worldY);
        (int, int) coordinates = Tile.TextureProcessor?.Process(configuration) ?? (0, 0);
        TextureX = coordinates.Item1;
        TextureY = coordinates.Item2;
    }
}

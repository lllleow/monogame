using Microsoft.Xna.Framework;
using MonoGame.Source.Systems.Tiles;

namespace MonoGame.Source.Rendering.Utils;

public class TextureLocation
{
    public string Spritesheet { get; set; }
    public Rectangle TextureRectangle { get; set; }

    public TextureLocation(string spritesheet, Rectangle textureRectangle)
    {
        Spritesheet = spritesheet;
        TextureRectangle = textureRectangle;
    }

    public static TextureLocation FirstTextureCoordinate(string spritesheet)
    {
        return new TextureLocation(spritesheet, new Rectangle(0, 0, Tile.PixelSizeX, Tile.PixelSizeY));
    }
}

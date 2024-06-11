using System.Drawing;

namespace MonoGame_Common.Util.Helpers;

public class TextureLocation
{
    public TextureLocation(string spritesheet, Rectangle textureRectangle)
    {
        Spritesheet = spritesheet;
        TextureRectangle = textureRectangle;
    }

    public string Spritesheet { get; set; }
    public Rectangle TextureRectangle { get; set; }

    public static TextureLocation FirstTextureCoordinate(string spritesheet)
    {
        return new TextureLocation(spritesheet, new Rectangle(0, 0, SharedGlobals.PixelSizeX, SharedGlobals.PixelSizeY));
    }
}
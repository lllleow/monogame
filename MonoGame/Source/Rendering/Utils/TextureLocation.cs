using Microsoft.Xna.Framework;
using MonoGame.Source.Systems.Tiles;

namespace MonoGame.Source.Rendering.Utils;

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
        return new TextureLocation(spritesheet, new Rectangle(0, 0, Globals.PixelSizeX, Globals.PixelSizeY));
    }
}
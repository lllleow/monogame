using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Source.Rendering.Utils;

public class TextureLocation
{
    public string Spritesheet;
    public Rectangle TextureRectangle;

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

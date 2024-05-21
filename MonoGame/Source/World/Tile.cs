using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class Tile : ITile
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SpritesheetName { get; set; }
    public int TextureX { get; set; }
    public int TextureY { get; set; }
    public Texture2D Texture { get; set; }
    public int SizeX { get; set; } = 1;
    public int SizeY { get; set; } = 1;
    public static int PixelSizeX { get; set; } = 32;
    public static int PixelSizeY { get; set; } = 32;

    public Tile()
    {

    }

    public void Initialize()
    {
        
    }
}

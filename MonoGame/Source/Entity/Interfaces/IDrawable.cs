using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public interface IDrawable
{
    string SpritesheetName { get; set; }
    public int PixelSizeX { get; set; }
    public int PixelSizeY { get; set; }
    int TextureX { get; set; }
    int TextureY { get; set; }
    void Draw(SpriteBatch spriteBatch);
}

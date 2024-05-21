using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public interface IDrawable
{
    string SpritesheetName { get; set; }
    Vector2 Size { get; set; }
    int TextureX { get; set; }
    int TextureY { get; set; }
    void Draw(SpriteBatch spriteBatch);
    void BaseDraw(SpriteBatch spriteBatch);
}

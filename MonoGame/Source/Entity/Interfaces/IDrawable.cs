using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public interface IDrawable
{
    Texture2D Texture { get; set; }
    Vector2 Size { get; set; }
    void Draw(SpriteBatch spriteBatch);
    void BaseDraw(SpriteBatch spriteBatch);
}

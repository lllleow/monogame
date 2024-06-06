using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Source.Systems.Entity.Interfaces;

public interface IDrawable
{

    string SpritesheetName { get; set; }

    int PixelSizeX { get; set; }

    int PixelSizeY { get; set; }

    int TextureX { get; set; }

    int TextureY { get; set; }

    void Draw(SpriteBatch spriteBatch);
}
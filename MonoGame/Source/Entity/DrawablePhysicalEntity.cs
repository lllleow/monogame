using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class DrawablePhysicalEntity : PhysicalEntity, IDrawable
{
    public int PixelSizeX { get; set; }
    public int PixelSizeY { get; set; }
    public string SpritesheetName { get; set; }
    public int TextureX { get; set; }
    public int TextureY { get; set; }

    public Rectangle GetCurrentSpriteRectangle()
    {
        return new Rectangle(0, 0, PixelSizeX, PixelSizeY);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!ContainsComponent<AnimatorComponent>())
        {
            spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(SpritesheetName), Position, GetCurrentSpriteRectangle(), Color.White);
        }

        foreach (var component in components)
        {
            component.Draw(spriteBatch);
        }
    }

    public void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        foreach (var component in components)
        {
            component.Update(gameTime);
        }
    }
}

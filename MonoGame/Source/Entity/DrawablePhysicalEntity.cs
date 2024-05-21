using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class DrawablePhysicalEntity : PhysicalEntity, IDrawable
{
    public Vector2 Size { get; set; }
    public string SpritesheetName { get; set; }
    public int TextureX { get; set; }
    public int TextureY { get; set; }

    public void BaseDraw(SpriteBatch spriteBatch)
    {
        foreach (var component in components)
        {
            component.Draw(spriteBatch);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        throw new NotImplementedException();
    }

    public override void Update(GameTime gameTime)
    {
        throw new NotImplementedException();
    }
}

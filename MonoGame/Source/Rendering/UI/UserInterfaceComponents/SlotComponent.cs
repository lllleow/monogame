using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Util.Loaders;

namespace MonoGame;

public class SlotComponent : UserInterfaceComponent, ISlotComponent
{
    public TextureLocation SlotTexture;

    public SlotComponent(string name, Rectangle bounds) : base(name, bounds)
    {
        SlotTexture = TextureLocation.FirstTextureCoordinate("textures/slot");
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        TextureLocation textureLocation = GetDrawable();
        Rectangle bounds = GetBounds();
        bounds.Location = ParentComponent != null ? ParentComponent.GetBounds().Location + bounds.Location : bounds.Location;

        spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(SlotTexture.Spritesheet), new Vector2(bounds.Location.X, bounds.Location.Y), SlotTexture.TextureRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);

        if (textureLocation != null)
        {
            spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(textureLocation.Spritesheet), new Vector2(bounds.Location.X, bounds.Location.Y), textureLocation.TextureRectangle, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
        }
    }

    public virtual TextureLocation GetDrawable()
    {
        throw new NotImplementedException();
    }
}

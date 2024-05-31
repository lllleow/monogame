using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Util.Loaders;

namespace MonoGame;

public class SlotComponent : UserInterfaceComponent, ISlotComponent
{
    public TextureLocation SlotTexture = TextureLocation.FirstTextureCoordinate("textures/slot");

    public SlotComponent(string name, Vector2 localPosition) : base(name, localPosition)
    {

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        TextureLocation textureLocation = GetDrawable();
        Vector2 position = GetPositionRelativeToParent();
        Vector2 size = GetPreferredSize();

        spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(SlotTexture.Spritesheet), new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), SlotTexture.TextureRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);

        if (textureLocation != null)
        {
            spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(textureLocation.Spritesheet), new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), textureLocation.TextureRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
        }
    }

    public virtual TextureLocation GetDrawable()
    {
        throw new NotImplementedException();
    }

    public override Vector2 GetPreferredSize()
    {
        return new Vector2(16, 16);
    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Util.Loaders;

namespace MonoGame;

public class SlotComponent : UserInterfaceComponent, ISlotComponent
{
    public TextureLocation SlotTexture = TextureLocation.FirstTextureCoordinate("textures/slot");

    public SlotComponent(string name, Vector2 position, Vector2 size) : base(name, position, size)
    {

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        TextureLocation textureLocation = GetDrawable();
        Vector2 position = GetRelativePosition();

        spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(SlotTexture.Spritesheet), new Rectangle((int)position.X, (int)position.Y, (int)Size.X, (int)Size.Y), SlotTexture.TextureRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);

        if (textureLocation != null)
        {
            spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(textureLocation.Spritesheet), new Rectangle((int)position.X, (int)position.Y, (int)Size.X, (int)Size.Y), textureLocation.TextureRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
        }
    }

    public virtual TextureLocation GetDrawable()
    {
        throw new NotImplementedException();
    }
}

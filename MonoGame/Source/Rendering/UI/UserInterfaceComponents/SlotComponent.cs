
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Rendering.Utils;
using MonoGame.Source.Util.Loaders;

namespace MonoGame;

public class SlotComponent : UserInterfaceComponent, ISlotComponent
{
    public TextureLocation SlotTexture = TextureLocation.FirstTextureCoordinate("textures/slot");
    public bool IsSelected = false;
    public RectangleHelper rectangleHelper = new RectangleHelper();

    public SlotComponent(string name, Vector2 localPosition) : base(name, localPosition)
    {
        IsClickable = true;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        TextureLocation textureLocation = GetDrawable();
        Vector2 position = GetPositionRelativeToParent();
        Vector2 size = GetPreferredSize();

        if (IsSelected)
        {
            SlotTexture.TextureRectangle = rectangleHelper.GetTextureRectangleFromCoordinates(1, 0);
        }
        else
        {
            SlotTexture.TextureRectangle = rectangleHelper.GetTextureRectangleFromCoordinates(0, 0);
        }

        spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(SlotTexture.Spritesheet), new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), SlotTexture.TextureRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
        Vector2 iconSize = size * 0.75f;

        if (textureLocation != null)
        {
            Vector2 iconPosition = new Vector2(position.X + ((size.X / 2) - (iconSize.X / 2)), position.Y + ((size.Y / 2) - (iconSize.Y / 2)));
            spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(textureLocation.Spritesheet), new Rectangle((int)iconPosition.X, (int)iconPosition.Y, (int)iconSize.X, (int)iconSize.Y), textureLocation.TextureRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
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

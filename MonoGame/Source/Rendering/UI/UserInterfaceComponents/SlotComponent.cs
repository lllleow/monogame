using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Common.Util.Helpers;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Rendering.Utils;
using MonoGame.Source.Utils.Helpers;
using MonoGame.Source.Utils.Loaders;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class SlotComponent : UserInterfaceComponent, ISlotComponent
{
    public TextureLocation SlotTexture { get; set; } = TextureLocation.FirstTextureCoordinate("textures/slot");
    public bool IsSelected { get; set; } = false;
    public RectangleHelper RectangleHelper { get; set; } = new();

    public SlotComponent(string name, Vector2 localPosition) : base(name, localPosition)
    {
        IsClickable = true;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        var textureLocation = GetDrawable();
        var position = GetPositionRelativeToParent();
        var size = GetPreferredSize();

        SlotTexture.TextureRectangle = IsSelected
            ? RectangleHelper.GetTextureRectangleFromCoordinates(1, 0)
            : RectangleHelper.GetTextureRectangleFromCoordinates(0, 0);

        spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(SlotTexture.Spritesheet), new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), SlotTexture.TextureRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
        var iconSize = size * 0.75f;

        if (textureLocation != null)
        {
            var iconPosition = new Vector2(position.X + ((size.X / 2) - (iconSize.X / 2)), position.Y + ((size.Y / 2) - (iconSize.Y / 2)));
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

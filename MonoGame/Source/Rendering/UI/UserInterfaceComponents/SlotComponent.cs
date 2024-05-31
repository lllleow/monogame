using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Source.Util.Loaders;

namespace MonoGame;

public class SlotComponent : UserInterfaceComponent, ISlotComponent
{
    public TextureLocation SlotTexture = TextureLocation.FirstTextureCoordinate("textures/slot");
    MouseState currentMouseState;
    public Action OnClick;

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

        Vector2 iconSize = size * 0.75f;

        if (textureLocation != null)
        {
            Vector2 iconPosition = new Vector2(position.X + ((size.X / 2) - (iconSize.X / 2)), position.Y + ((size.Y / 2) - (iconSize.Y / 2)));
            spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(textureLocation.Spritesheet), new Rectangle((int)iconPosition.X, (int)iconPosition.Y, (int)iconSize.X, (int)iconSize.Y), textureLocation.TextureRectangle, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1f);
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        currentMouseState = Mouse.GetState();
        if (currentMouseState.LeftButton == ButtonState.Pressed || currentMouseState.RightButton == ButtonState.Pressed)
        {
            int mouseX = currentMouseState.X;
            int mouseY = currentMouseState.Y;

            HandleMouseClick(currentMouseState.LeftButton == ButtonState.Pressed, mouseX, mouseY);
        }
    }

    private void HandleMouseClick(bool add, int x, int y)
    {
        int windowWidth = Globals.graphicsDevice.PreferredBackBufferWidth;
        int windowHeight = Globals.graphicsDevice.PreferredBackBufferHeight;

        if (!Globals.game.IsActive)
        {
            return;
        }

        if (x < 0 || y < 0 || x >= windowWidth || y >= windowHeight)
        {
            return;
        }

        Vector2 worldPosition = new Vector2(x, y);
        Vector2 screenPosition = Vector2.Transform(worldPosition, Matrix.Invert(Globals.userInterfaceHandler.GetUITransform()));

        if (screenPosition.X >= GetPositionRelativeToParent().X && screenPosition.X <= GetPositionRelativeToParent().X + GetPreferredSize().X && screenPosition.Y >= GetPositionRelativeToParent().Y && screenPosition.Y <= GetPositionRelativeToParent().Y + GetPreferredSize().Y)
        {
            OnClick();
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

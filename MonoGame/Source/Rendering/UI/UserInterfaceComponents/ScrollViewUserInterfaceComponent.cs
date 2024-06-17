using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame_Common;
using MonoGame.Source;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;

namespace MonoGame;

public class ScrollViewUserInterfaceComponent : DirectionalListUserInterfaceComponent
{
    public Vector2 ContentOffset { get; set; } = Vector2.Zero;
    private ScrollViewIndicatorUserIntefaceComponent scrollViewIndicator;
    private IUserInterfaceComponent child;

    public ScrollViewUserInterfaceComponent(Vector2 size, IUserInterfaceComponent child) : base("scroll_view", Axis.Horizontal, Vector2.Zero, 0, [])
    {
        SizeOverride = size;
        scrollViewIndicator = new ScrollViewIndicatorUserIntefaceComponent(size.Y);

        this.child = child;

        AddChild(child);
        AddChild(scrollViewIndicator);

        float sizeOverridePercentageOfContentSize = SizeOverride.Y / GetContentSize().Y;
        scrollViewIndicator.SetChild(new ScrollViewIndicatorUserIntefaceComponent(SizeOverride.Y * sizeOverridePercentageOfContentSize)
        {
            BackgroundImage = "textures/ui_background_selected",
            BackgroundImageMode = UserInterfaceBackgroundImageMode.Tile
        });
    }

    public Vector2 GetContentSize()
    {
        return child.GetPreferredSize();
    }

    public override void Initialize(IUserInterfaceComponent parent)
    {
        base.Initialize(parent);
        InputEventManager.Subscribe(InputEventChannel.UI, inputEvent =>
        {
            if (!Enabled) return;
            if (inputEvent.EventType == InputEventType.MouseScrolled && MouseIntersectsComponent())
            {
                inputEvent.Handled = true;
                float yOffset = inputEvent.ScrollDelta / 10;

                Vector2 newContentOffset = ContentOffset + new Vector2(0, yOffset);
                int border = 4;
                if (newContentOffset.Y < -GetContentSize().Y + border)
                {
                    newContentOffset = new Vector2(newContentOffset.X, -GetContentSize().Y + border);
                }

                if (newContentOffset.Y > 0)
                {
                    newContentOffset = new Vector2(newContentOffset.X, 0);
                }

                ContentOffset = newContentOffset;

                float sizeOverridePercentageOfContentSize = SizeOverride.Y / GetContentSize().Y;
                float percentageScrolled = ContentOffset.Y / GetContentSize().Y;
                scrollViewIndicator.SetChild(new ScrollViewIndicatorUserIntefaceComponent(SizeOverride.Y * sizeOverridePercentageOfContentSize)
                {
                    LocalPosition = new Vector2(0, -(percentageScrolled * (SizeOverride.Y - (SizeOverride.Y * sizeOverridePercentageOfContentSize)))),
                    BackgroundImage = "textures/ui_background_selected",
                    BackgroundImageMode = UserInterfaceBackgroundImageMode.Tile
                });
            }
        });
    }

    public override Vector2 GetPreferredSize()
    {
        Vector2 childSize = new Vector2(0, 0);
        foreach (IUserInterfaceComponent child in Children)
        {
            childSize += child.GetPreferredSize();
        }

        if (SizeOverride != Vector2.Zero)
        {
            if (SizeOverride.X < 0 && SizeOverride.Y > 0)
            {
                return new Vector2(childSize.X, SizeOverride.Y);
            }
            else if (SizeOverride.X > 0 && SizeOverride.Y < 0)
            {
                return new Vector2(SizeOverride.X, childSize.Y);
            }

            return SizeOverride;
        }
        else
        {
            return childSize;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Vector2 uiPosition = GetPositionRelativeToParent();
        Vector2 uiSize = GetPreferredSize();
        Vector2 position = Vector2.Transform(uiPosition, Globals.UserInterfaceHandler.GetUITransform());
        Vector2 size = Vector2.Transform(uiSize, Globals.UserInterfaceHandler.GetUITransform());
        RasterizerState rasterizerState = new RasterizerState() { ScissorTestEnable = true };
        Globals.GraphicsDevice.GraphicsDevice.RasterizerState = rasterizerState;
        Globals.GraphicsDevice.GraphicsDevice.ScissorRectangle = new Microsoft.Xna.Framework.Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        spriteBatch.End();
        spriteBatch.Begin(transformMatrix: Globals.UserInterfaceHandler.Transform, sortMode: SpriteSortMode.Deferred, blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, rasterizerState: rasterizerState);
        base.Draw(spriteBatch);
        spriteBatch.End();
        Globals.DefaultSpriteBatchUIBegin();
        Globals.GraphicsDevice.GraphicsDevice.ScissorRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, Globals.GraphicsDevice.GraphicsDevice.Viewport.Width, Globals.GraphicsDevice.GraphicsDevice.Viewport.Height);
    }

    public override Vector2 GetChildOffset(IUserInterfaceComponent child)
    {
        if (child == scrollViewIndicator)
        {
            return base.GetChildOffset(child);
        }
        else
        {
            return base.GetChildOffset(child) + ContentOffset;
        }
    }
}

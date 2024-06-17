using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame_Common;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Rendering.Utils;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class UserInterfaceComponent : IUserInterfaceComponent
{
    private readonly PrimitiveBatch primitiveBatch =
        new(Globals.GraphicsDevice.GraphicsDevice, Globals.UserInterfaceHandler.Transform);
    public Vector2 SizeOverride { get; set; } = Vector2.Zero;
    public float Opacity { get; set; } = 1f;

    public UserInterfaceComponent(string name, Vector2 localPosition)
    {
        Name = name;
        LocalPosition = localPosition;
    }

    public bool IsClickable { get; set; } = false;
    public IUserInterfaceComponent Parent { get; set; }
    public static bool ShowBounds { get; set; } = false;
    public static List<Type> BoundRenderOffForTypes { get; set; } = [typeof(PaddingUserInterfaceComponent)];
    public MouseState CurrentMouseState { get; set; }
    public MouseState PreviousMouseState { get; set; }
    public Action<IUserInterfaceComponent> OnClick { get; set; }
    public string Name { get; set; }
    public Vector2 LocalPosition { get; set; }
    public bool Enabled { get; set; } = true;

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (!Enabled) return;
        if (ShowBounds && !BoundRenderOffForTypes.Contains(GetType()))
        {
            primitiveBatch.Begin(PrimitiveType.LineList, Globals.UserInterfaceHandler.Transform);

            var position = GetPositionRelativeToParent();
            var size = GetPreferredSize();

            var rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            var topLeft = rectangle.Location.ToVector2();
            var topRight = new Vector2(rectangle.Right, rectangle.Top);
            var bottomLeft = new Vector2(rectangle.Left, rectangle.Bottom);
            var bottomRight = new Vector2(rectangle.Right, rectangle.Bottom);

            primitiveBatch.AddVertex(topLeft, Color.Red);
            primitiveBatch.AddVertex(topRight, Color.Red);

            primitiveBatch.AddVertex(topRight, Color.Red);
            primitiveBatch.AddVertex(bottomRight, Color.Red);

            primitiveBatch.AddVertex(bottomRight, Color.Red);
            primitiveBatch.AddVertex(bottomLeft, Color.Red);

            primitiveBatch.AddVertex(bottomLeft, Color.Red);
            primitiveBatch.AddVertex(topLeft, Color.Red);

            primitiveBatch.End();
        }
    }

    public bool IsClicked { get; set; } = false;
    public Vector2 CalculatedSize { get; set; }

    public virtual void Update(GameTime gameTime)
    {
        if (!Enabled) return;
        CurrentMouseState = Mouse.GetState();
    }

    public virtual void Initialize(IUserInterfaceComponent parent)
    {
        Parent = parent;
        InputEventManager.Subscribe(InputEventChannel.UI, inputEvent =>
        {
            if (OnClick == null || !Enabled) return;

            if (inputEvent.EventType == InputEventType.MouseButtonDown)
            {
                if (inputEvent.Button == MouseButton.Left && MouseIntersectsComponent())
                {
                    inputEvent.Handled = true;
                    IsClicked = true;
                }
            }

            if (inputEvent.EventType == InputEventType.MouseButtonUp)
            {
                if (inputEvent.Button == MouseButton.Left && MouseIntersectsComponent() && IsClicked)
                {
                    IsClicked = false;
                    inputEvent.Handled = true;
                    OnClick?.Invoke(this);
                }
            }
        });
    }

    public virtual Vector2 GetPositionRelativeToParent()
    {
        return (Parent?.GetPositionRelativeToParent() + LocalPosition ?? LocalPosition) +
               (Parent?.GetChildOffset(this) ?? Vector2.Zero);
    }

    public virtual Vector2 GetPreferredSize()
    {
        throw new NotImplementedException();
    }

    public virtual Vector2 GetChildOffset(IUserInterfaceComponent child)
    {
        return Vector2.Zero;
    }

    public bool MouseIntersectsComponent()
    {
        CurrentMouseState = Mouse.GetState();
        var x = CurrentMouseState.X;
        var y = CurrentMouseState.Y;

        var windowWidth = Globals.GraphicsDevice.PreferredBackBufferWidth;
        var windowHeight = Globals.GraphicsDevice.PreferredBackBufferHeight;

        if (!Globals.Game.IsActive)
        {
            return false;
        }

        if (x < 0 || y < 0 || x >= windowWidth || y >= windowHeight)
        {
            return false;
        }

        var worldPosition = new Vector2(x, y);
        var screenPosition =
            Vector2.Transform(worldPosition, Matrix.Invert(Globals.UserInterfaceHandler.GetUITransform()));

        return screenPosition.X >= GetPositionRelativeToParent().X &&
               screenPosition.X <= GetPositionRelativeToParent().X + GetPreferredSize().X &&
               screenPosition.Y >= GetPositionRelativeToParent().Y &&
               screenPosition.Y <= GetPositionRelativeToParent().Y + GetPreferredSize().Y;
    }

    public int GetPercentageOfScreenWidth(float percent)
    {
        float screenHeight = Globals.GraphicsDevice.PreferredBackBufferHeight;
        float screenWidth = Globals.GraphicsDevice.PreferredBackBufferWidth;
        Vector2 screenSize = new Vector2(screenWidth, screenHeight);
        Vector2 uiSize = Vector2.Transform(screenSize, Matrix.Invert(Globals.UserInterfaceHandler.GetUITransform()));
        return (int)(uiSize.X * percent);
    }

    public int GetPercentageOfScreenHeight(float percent)
    {
        float screenHeight = Globals.GraphicsDevice.PreferredBackBufferHeight;
        float screenWidth = Globals.GraphicsDevice.PreferredBackBufferWidth;
        Vector2 screenSize = new Vector2(screenWidth, screenHeight);
        Vector2 uiSize = Vector2.Transform(screenSize, Matrix.Invert(Globals.UserInterfaceHandler.GetUITransform()));
        return (int)(uiSize.Y * percent);
    }
}
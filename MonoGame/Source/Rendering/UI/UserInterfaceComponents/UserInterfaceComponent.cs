using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Rendering.Utils;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class UserInterfaceComponent : IUserInterfaceComponent
{
    public string Name { get; set; }
    public bool IsClickable { get; set; } = false;
    public Vector2 LocalPosition { get; set; }
    public IUserInterfaceComponent Parent { get; set; }
    public static bool ShowBounds { get; set; } = false;
    public static List<Type> BoundRenderOffForTypes { get; set; } = [typeof(PaddingUserInterfaceComponent)];
    public MouseState CurrentMouseState { get; set; }
    public MouseState PreviousMouseState { get; set; }
    public Action<IUserInterfaceComponent> OnClick { get; set; }

    public UserInterfaceComponent(string name, Vector2 localPosition)
    {
        Name = name;
        LocalPosition = localPosition;
    }

    private readonly PrimitiveBatch primitiveBatch = new(Globals.GraphicsDevice.GraphicsDevice, transform: Globals.UserInterfaceHandler.Transform);
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (ShowBounds && !BoundRenderOffForTypes.Contains(GetType()))
        {
            primitiveBatch.Begin(PrimitiveType.LineList, transform: Globals.UserInterfaceHandler.Transform);

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

    public virtual void Update(GameTime gameTime)
    {
        CurrentMouseState = Mouse.GetState();

        if (CurrentMouseState.LeftButton == ButtonState.Pressed || CurrentMouseState.RightButton == ButtonState.Pressed)
        {
            var mouseX = CurrentMouseState.X;
            var mouseY = CurrentMouseState.Y;

            HandleMouseClick(CurrentMouseState.LeftButton == ButtonState.Pressed, mouseX, mouseY);
        }
    }

    private void HandleMouseClick(bool add, int x, int y)
    {
        var windowWidth = Globals.GraphicsDevice.PreferredBackBufferWidth;
        var windowHeight = Globals.GraphicsDevice.PreferredBackBufferHeight;

        if (!Globals.Game.IsActive)
        {
            return;
        }

        if (x < 0 || y < 0 || x >= windowWidth || y >= windowHeight)
        {
            return;
        }

        var worldPosition = new Vector2(x, y);
        var screenPosition = Vector2.Transform(worldPosition, Matrix.Invert(Globals.UserInterfaceHandler.GetUITransform()));

        if (screenPosition.X >= GetPositionRelativeToParent().X && screenPosition.X <= GetPositionRelativeToParent().X + GetPreferredSize().X && screenPosition.Y >= GetPositionRelativeToParent().Y && screenPosition.Y <= GetPositionRelativeToParent().Y + GetPreferredSize().Y)
        {
            OnClick?.Invoke(this);
        }
    }

    public virtual void Initialize(IUserInterfaceComponent parent)
    {
        Parent = parent;
    }

    public virtual Vector2 GetPositionRelativeToParent()
    {
        return (Parent?.GetPositionRelativeToParent() + LocalPosition ?? LocalPosition) + (Parent?.GetChildOffset(this) ?? Vector2.Zero);
    }

    public virtual Vector2 GetPreferredSize()
    {
        throw new NotImplementedException();
    }

    public virtual Vector2 GetChildOffset(IUserInterfaceComponent child)
    {
        return Vector2.Zero;
    }
}

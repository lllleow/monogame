using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Source.Rendering.UI.Interfaces;

namespace MonoGame;

public class UserInterfaceComponent : IUserInterfaceComponent
{
    public string Name { get; set; }
    public bool IsClickable { get; set; } = false;
    public Vector2 LocalPosition { get; set; }
    public IUserInterfaceComponent Parent { get; set; }
    public static bool ShowBounds { get; set; } = false;
    public static List<Type> BoundRenderOffForTypes = new List<Type>() { typeof(PaddingUserInterfaceComponent) };
    public MouseState currentMouseState;
    public MouseState previousMouseState;
    public Action<IUserInterfaceComponent> OnClick;

    public UserInterfaceComponent(string name, Vector2 localPosition)
    {
        Name = name;
        LocalPosition = localPosition;
    }

    PrimitiveBatch primitiveBatch = new PrimitiveBatch(Globals.GraphicsDevice.GraphicsDevice, transform: Globals.UserInterfaceHandler.Transform);
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (ShowBounds && !BoundRenderOffForTypes.Contains(this.GetType()))
        {
            primitiveBatch.Begin(PrimitiveType.LineList, transform: Globals.UserInterfaceHandler.Transform);

            Vector2 position = GetPositionRelativeToParent();
            Vector2 size = GetPreferredSize();

            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            Vector2 topLeft = rectangle.Location.ToVector2();
            Vector2 topRight = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 bottomLeft = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 bottomRight = new Vector2(rectangle.Right, rectangle.Bottom);

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
        int windowWidth = Globals.GraphicsDevice.PreferredBackBufferWidth;
        int windowHeight = Globals.GraphicsDevice.PreferredBackBufferHeight;

        if (!Globals.Game.IsActive)
        {
            return;
        }

        if (x < 0 || y < 0 || x >= windowWidth || y >= windowHeight)
        {
            return;
        }

        Vector2 worldPosition = new Vector2(x, y);
        Vector2 screenPosition = Vector2.Transform(worldPosition, Matrix.Invert(Globals.UserInterfaceHandler.GetUITransform()));

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

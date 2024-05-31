using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class UserInterfaceComponent : IUserInterfaceComponent
{
    public string Name { get; set; }
    public Vector2 LocalPosition { get; set; }
    public IUserInterfaceComponent Parent { get; set; }
    public static bool ShowBounds { get; set; } = true;
    public static List<Type> BoundRenderOffForTypes = new List<Type>() { typeof(PaddingUserInterfaceComponent) };

    public UserInterfaceComponent(string name, Vector2 localPosition)
    {
        Name = name;
        LocalPosition = localPosition;
    }

    PrimitiveBatch primitiveBatch = new PrimitiveBatch(Globals.graphicsDevice.GraphicsDevice, transform: Globals.userInterfaceHandler.Transform);
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (ShowBounds && !BoundRenderOffForTypes.Contains(this.GetType()))
        {
            primitiveBatch.Begin(PrimitiveType.LineList, transform: Globals.userInterfaceHandler.Transform);

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

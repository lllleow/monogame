using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class UserInterfaceComponent : IUserInterfaceComponent
{
    public string Name { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
    public Vector2 ContentPadding { get; set; }
    public IUserInterfaceComponent Parent { get; set; }
    public static bool ShowBounds { get; set; } = true;

    public UserInterfaceComponent(string name, Vector2 position, Vector2 size)
    {
        Name = name;
        Position = position;
        Size = size;
    }

    public UserInterfaceComponent(string name, Vector2 position, Vector2 size, Vector2 contentPadding)
    {
        Name = name;
        Position = position;
        Size = size;
        ContentPadding = contentPadding;
    }

    public virtual Vector2 GetRelativePosition()
    {
        Vector2 parentPosition = Parent?.Position ?? Vector2.Zero;
        Vector2 relativePosition = parentPosition + Position;
        relativePosition += GetRelativeContentPadding() / 2;

        if (Parent is IParentUserInterfaceComponent parentUserInterfaceComponent)
        {
            relativePosition += parentUserInterfaceComponent.GetOriginForAlignment(Size);
        }

        if (Parent is IMultipleChildUserInterfaceComponent multipleChildUserInterfaceComponent)
        {
            relativePosition += multipleChildUserInterfaceComponent.GetOffsetForChild(this);
        }

        return relativePosition;
    }

    PrimitiveBatch primitiveBatch = new PrimitiveBatch(Globals.graphicsDevice.GraphicsDevice, transform: Globals.userInterfaceHandler.Transform);
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (ShowBounds)
        {
            primitiveBatch.Begin(PrimitiveType.LineList, transform: Globals.userInterfaceHandler.Transform);

            Vector2 position = GetRelativePosition();
            Rectangle rectangle = new Rectangle((int)position.X, (int)position.Y, (int)Size.X, (int)Size.Y);
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

    public Vector2 GetRelativeContentPadding()
    {
        return ContentPadding + (Parent?.GetRelativeContentPadding() ?? Vector2.Zero);
    }
}

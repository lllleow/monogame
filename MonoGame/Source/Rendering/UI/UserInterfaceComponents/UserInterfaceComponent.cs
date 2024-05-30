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
        Vector2 relativePosition = (Parent?.Position ?? Vector2.Zero) + Position;
        relativePosition += GetRelativeContentPadding() / 2;

        if (Parent is IMultipleChildUserInterfaceComponent multipleChildUserInterfaceComponent)
        {
            relativePosition += multipleChildUserInterfaceComponent.GetOffsetForChild(this);
        }

        return relativePosition;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {

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

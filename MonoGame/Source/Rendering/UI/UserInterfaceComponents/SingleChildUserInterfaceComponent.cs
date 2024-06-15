using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Rendering.UI.Interfaces;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class SingleChildUserInterfaceComponent : UserInterfaceComponent
{
    public SingleChildUserInterfaceComponent(string name, Vector2 localPosition, IUserInterfaceComponent child) : base(name, localPosition)
    {
        child?.Initialize(this);
        Child = child;
    }

    public IUserInterfaceComponent Child { get; set; }

    public void RemoveChild()
    {
        Child = null;
    }

    public void SetChild(IUserInterfaceComponent child)
    {
        child?.Initialize(this);
        Child = child;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        Child?.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        Child?.Update(gameTime);
    }

    public override Vector2 GetPreferredSize()
    {
        Vector2 childSize = (Child?.GetPreferredSize() ?? Vector2.Zero) + (Child?.LocalPosition ?? Vector2.Zero);
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
}
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class SingleChildUserInterfaceComponent : UserInterfaceComponent
{
    public IUserInterfaceComponent Child { get; set; }

    public SingleChildUserInterfaceComponent(string name, Vector2 localPosition, IUserInterfaceComponent child) : base(name, localPosition)
    {
        child.Initialize(this);
        Child = child;
    }

    public void RemoveChild()
    {
        Child = null;
    }

    public void SetChild(IUserInterfaceComponent child)
    {
        child.Initialize(this);
        Child = child;
    }

    public override void Draw(SpriteBatch spriteBatch, Vector2 origin)
    {
        base.Draw(spriteBatch, origin);
        Child?.Draw(spriteBatch, origin);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        Child?.Update(gameTime);
    }

    public override Vector2 GetPreferredSize()
    {
        return Child?.GetPreferredSize() ?? Vector2.Zero;
    }
}

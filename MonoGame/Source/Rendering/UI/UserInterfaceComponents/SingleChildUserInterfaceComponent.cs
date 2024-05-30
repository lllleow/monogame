using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class SingleChildUserInterfaceComponent : UserInterfaceComponent, ISingleChildUserInterfaceComponent
{
    public IUserInterfaceComponent Child { get; set; }
    public SingleChildUserInterfaceComponent(string name, Vector2 position, Vector2 size, IUserInterfaceComponent child) : base(name, position, size)
    {
        child.Initialize(this);
        Child = child;
    }

    public SingleChildUserInterfaceComponent(string name, Vector2 position, Vector2 size, Vector2 contentPadding, IUserInterfaceComponent child) : base(name, position, size, contentPadding)
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
}

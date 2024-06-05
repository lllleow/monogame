using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Rendering.UI.Interfaces;

namespace MonoGame;

public class SingleChildUserInterfaceComponent : UserInterfaceComponent
{
    public IUserInterfaceComponent Child { get; set; }

    public SingleChildUserInterfaceComponent(string name, Vector2 localPosition, IUserInterfaceComponent child) : base(name, localPosition)
    {
        child?.Initialize(this);
        Child = child;
    }

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
        return (Child?.GetPreferredSize() ?? Vector2.Zero) + (Child?.LocalPosition ?? Vector2.Zero);
    }
}

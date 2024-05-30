using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class ContainerUserInterfaceComponent : SingleChildUserInterfaceComponent
{
    public ContainerUserInterfaceComponent(string name, Vector2 position, Vector2 size, UserInterfaceAlignment childAlignment, IUserInterfaceComponent child) : base(name, position, size, childAlignment, child)
    {
    }

    public ContainerUserInterfaceComponent(string name, Vector2 position, Vector2 size, Vector2 contentPadding, UserInterfaceAlignment childAlignment, IUserInterfaceComponent child) : base(name, position, size, contentPadding, childAlignment, child)
    {
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Child?.Draw(spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        Child?.Update(gameTime);
    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class ContainerUserInterfaceComponent : SingleChildUserInterfaceComponent
{
    public ContainerUserInterfaceComponent(string name, Vector2 position, Vector2 size, IUserInterfaceComponent child) : base(name, position, size, child)
    {

    }

    public ContainerUserInterfaceComponent(string name, Vector2 position, Vector2 size, Vector2 contentPadding, IUserInterfaceComponent child) : base(name, position, size, contentPadding, child)
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

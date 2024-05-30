using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class ContainerUserInterfaceComponent : SingleChildUserInterfaceComponent
{
    public ContainerUserInterfaceComponent(string name, Vector2 position, Vector2 size, IUserInterfaceComponent child) : base(name, position, size, child)
    {

    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class ContainerUserInterfaceComponent : SingleChildUserInterfaceComponent
{
    public ContainerUserInterfaceComponent(Vector2 localPosition, IUserInterfaceComponent child) : base("container", localPosition, child)
    {

    }
}

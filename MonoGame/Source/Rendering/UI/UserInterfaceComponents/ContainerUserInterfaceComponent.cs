using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Rendering.UI.Interfaces;

namespace MonoGame;

public class ContainerUserInterfaceComponent : SingleChildUserInterfaceComponent
{
    public ContainerUserInterfaceComponent(Vector2 localPosition, IUserInterfaceComponent child) : base("container", localPosition, child)
    {

    }
}

using System;
using Microsoft.Xna.Framework;

namespace MonoGame;

public class SizedBoxUserInterfaceComponent : UserInterfaceComponent
{
    public SizedBoxUserInterfaceComponent(Vector2 size) : base("sized_box", Vector2.Zero, size)
    {
    }

    public SizedBoxUserInterfaceComponent(Vector2 size, Vector2 contentPadding) : base("sized_box", Vector2.Zero, size, contentPadding)
    {
    }
}

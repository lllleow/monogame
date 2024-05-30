using System;
using Microsoft.Xna.Framework;

namespace MonoGame;

public class SizedBoxUserInterfaceComponent : UserInterfaceComponent
{
    public Vector2 Size { get; set; }

    public SizedBoxUserInterfaceComponent(Vector2 localPosition, Vector2 size) : base("sized_box", localPosition)
    {
        Size = size;
    }

    public override Vector2 GetPreferredSize()
    {
        return Size;
    }
}

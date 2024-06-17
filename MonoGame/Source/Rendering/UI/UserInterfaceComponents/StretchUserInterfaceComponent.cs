using System;
using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Rendering.UI.UserInterfaceComponents;

namespace MonoGame;

public class StretchUserInterfaceComponent : SingleChildUserInterfaceComponent
{
    public Axis Axis { get; set; } = Axis.Horizontal;
    public StretchUserInterfaceComponent(Axis axis, IUserInterfaceComponent child) : base("stretch", new Vector2(0, 0), child)
    {
    }

    public override Vector2 GetPreferredSize()
    {
        if (Child is not UserInterfaceComponent userInterfaceComponent) return base.GetPreferredSize();

        Vector2 parentSize = Parent?.CalculatedSize ?? Vector2.Zero;
        Vector2 size = Vector2.Zero;

        Vector2 childSize = userInterfaceComponent.GetPreferredSize();
        if (Axis == Axis.Horizontal)
        {
            size = new Vector2(parentSize.X, childSize.Y);
        }
        else
        {
            size = new Vector2(childSize.X, parentSize.Y);
        }

        userInterfaceComponent.SizeOverride = size;
        CalculatedSize = size;
        return size;
    }
}

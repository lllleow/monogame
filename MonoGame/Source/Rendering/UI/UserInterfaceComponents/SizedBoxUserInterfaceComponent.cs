using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.UI.Interfaces;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class SizedBoxUserInterfaceComponent : SingleChildUserInterfaceComponent
{
    public SizedBoxUserInterfaceComponent(Vector2 localPosition, Vector2 size, IUserInterfaceComponent child) : base(
        "sized_box", localPosition, child)
    {
        Size = size;
    }

    public Vector2 Size { get; set; }

    public override Vector2 GetPreferredSize()
    {
        var childSize = base.GetPreferredSize();
        var size = Size.X < 0 && Size.Y < 0
            ? childSize
            : Size.Y > 0 && Size.X < 0
            ? new Vector2(childSize.X, Size.Y)
            : Size.X > 0 && Size.Y < 0 ? new Vector2(Size.X, childSize.Y) : Size;
        CalculatedSize = size;
        return size;
    }
}
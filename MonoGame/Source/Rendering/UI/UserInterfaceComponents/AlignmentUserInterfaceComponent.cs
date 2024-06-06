using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.UI.Interfaces;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class AlignmentUserInterfaceComponent : SingleChildUserInterfaceComponent
{
    public UserInterfaceAlignment Alignment { get; set; }

    public AlignmentUserInterfaceComponent(IUserInterfaceComponent child, UserInterfaceAlignment alignment) : base("alignment", Vector2.Zero, child)
    {
        Alignment = alignment;
    }

    public override Vector2 GetPositionRelativeToParent()
    {
        var parentPosition = Parent?.GetPositionRelativeToParent() ?? Vector2.Zero;
        var parentSize = Parent?.GetPreferredSize() ?? Globals.UserInterfaceHandler.UIScreenSize;
        var componentSize = GetPreferredSize();
        var position = Vector2.Zero;

        switch (Alignment)
        {
            case UserInterfaceAlignment.LeftUp:
                position = new Vector2(
                    parentPosition.X,
                    parentPosition.Y
                );
                break;
            case UserInterfaceAlignment.RightCenter:
                position = new Vector2(
                    parentPosition.X + parentSize.X - componentSize.X,
                    parentPosition.Y + (parentSize.Y / 2) - (componentSize.Y / 2)
                );
                break;
            case UserInterfaceAlignment.LeftCenter:
                position = new Vector2(
                    parentPosition.X,
                    parentPosition.Y + (parentSize.Y / 2) - (componentSize.Y / 2)
                );
                break;
            case UserInterfaceAlignment.LeftDown:
                position = new Vector2(
                    parentPosition.X,
                    parentPosition.Y + parentSize.Y - componentSize.Y
                );
                break;
            case UserInterfaceAlignment.CenterUp:
                position = new Vector2(
                    parentPosition.X + (parentSize.X / 2) - (componentSize.X / 2),
                    parentPosition.Y
                );
                break;
            case UserInterfaceAlignment.Center:
                position = new Vector2(
                    parentPosition.X + (parentSize.X / 2) - (componentSize.X / 2),
                    parentPosition.Y + (parentSize.Y / 2) - (componentSize.Y / 2)
                );
                break;
            case UserInterfaceAlignment.CenterDown:
                position = new Vector2(
                    parentPosition.X + (parentSize.X / 2) - (componentSize.X / 2),
                    parentPosition.Y + parentSize.Y - componentSize.Y
                );
                break;
            case UserInterfaceAlignment.RightUp:
                position = new Vector2(
                    parentPosition.X + parentSize.X - componentSize.X,
                    parentPosition.Y
                );
                break;
            case UserInterfaceAlignment.RightDown:
                position = new Vector2(
                    parentPosition.X + parentSize.X - componentSize.X,
                    parentPosition.Y + parentSize.Y - componentSize.Y
                );
                break;
        }

        return position;
    }
}

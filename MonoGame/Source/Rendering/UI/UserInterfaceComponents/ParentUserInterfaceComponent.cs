using System;
using Microsoft.Xna.Framework;

namespace MonoGame;

public class ParentUserInterfaceComponent : UserInterfaceComponent, IParentUserInterfaceComponent
{
    public ParentUserInterfaceComponent(string name, Vector2 position, Vector2 size, UserInterfaceAlignment childAlignment) : base(name, position, size)
    {
        ChildAlignment = childAlignment;
    }

    public ParentUserInterfaceComponent(string name, Vector2 position, Vector2 size, Vector2 contentPadding, UserInterfaceAlignment childAlignment) : base(name, position, size, contentPadding)
    {
        ChildAlignment = childAlignment;
    }

    public UserInterfaceAlignment ChildAlignment { get; set; }

    public virtual Vector2 GetChildPositionForAlignment(IUserInterfaceComponent component)
    {
        Vector2 origin;

        int componentWidth = (int)component.Size.X;
        int componentHeight = (int)component.Size.Y;

        if (this is IMultipleChildUserInterfaceComponent multiChild)
        {
            componentWidth = multiChild.GetBoundsOfChildren().Size.X;
            componentHeight = multiChild.GetBoundsOfChildren().Size.Y;
        }

        int centerX = (int)((Size.X / 2) - (componentWidth / 2));
        int centerY = (int)((Size.Y / 2) - (componentHeight / 2));

        switch (ChildAlignment)
        {
            case UserInterfaceAlignment.LeftCenter:
                origin = new Vector2(0, centerY);
                break;
            case UserInterfaceAlignment.CenterUp:
                origin = new Vector2(centerX, 0);
                break;
            case UserInterfaceAlignment.Center:
                origin = new Vector2(centerX, centerY);
                break;
            case UserInterfaceAlignment.RightCenter:
                origin = new Vector2(Size.X - componentWidth, centerY);
                break;
            case UserInterfaceAlignment.CenterDown:
                origin = new Vector2(centerX, Size.Y - componentHeight);
                break;
            case UserInterfaceAlignment.LeftUp:
                origin = new Vector2(0, 0);
                break;
            case UserInterfaceAlignment.RightUp:
                origin = new Vector2(Size.X - componentWidth, 0);
                break;
            case UserInterfaceAlignment.LeftDown:
                origin = new Vector2(0, Size.Y - componentHeight);
                break;
            case UserInterfaceAlignment.RightDown:
                origin = new Vector2(Size.X - componentWidth, Size.Y - componentHeight);
                break;
            default:
                origin = new Vector2(Position.X, Position.Y);
                break;
        }

        if (Parent is IParentUserInterfaceComponent parentUserInterfaceComponent)
        {
            origin += parentUserInterfaceComponent.GetPositionForAlignment(this);
            return origin;
        }
        else
        {
            return origin;
        }
    }
}

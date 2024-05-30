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

    public virtual Vector2 GetOriginForAlignment(Vector2 componentSize)
    {
        Vector2 origin;
        int SizeX = (int)Size.X;
        int SizeY = (int)Size.Y;

        int componentWidth = (int)componentSize.X;
        int componentHeight = (int)componentSize.Y;

        if (this is IMultipleChildUserInterfaceComponent multipleChildUserInterfaceComponent)
        {
            Rectangle childrenSize = multipleChildUserInterfaceComponent.GetBoundsOfChildren();
            componentWidth = (int)childrenSize.Size.X;
            componentHeight = (int)childrenSize.Size.Y;
        }

        int centerX = (int)((SizeX / 2) - (componentWidth / 2));
        int centerY = (int)((SizeY / 2) - (componentHeight / 2));

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
                origin = new Vector2(SizeX - componentWidth, centerY);
                break;
            case UserInterfaceAlignment.CenterDown:
                origin = new Vector2(centerX, SizeY - componentHeight);
                break;
            case UserInterfaceAlignment.LeftUp:
                origin = new Vector2(0, 0);
                break;
            case UserInterfaceAlignment.RightUp:
                origin = new Vector2(SizeX - componentWidth, 0);
                break;
            case UserInterfaceAlignment.LeftDown:
                origin = new Vector2(0, SizeY - componentHeight);
                break;
            case UserInterfaceAlignment.RightDown:
                origin = new Vector2(SizeX - componentWidth, SizeY - componentHeight);
                break;
            default:
                origin = new Vector2(Position.X, Position.Y);
                break;
        }

        if (Parent is IParentUserInterfaceComponent parentUserInterfaceComponent)
        {
            origin += parentUserInterfaceComponent.GetOriginForAlignment(Size);
            return origin;
        }

        return origin;
    }
}

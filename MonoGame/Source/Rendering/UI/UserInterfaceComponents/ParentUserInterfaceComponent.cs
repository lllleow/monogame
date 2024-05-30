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

    public virtual Vector2 GetOriginForAlignment()
    {
        Vector2 parentSize = Parent?.Size ?? Vector2.Zero;
        Vector2 origin = Vector2.Zero;

        if (Parent is IParentUserInterfaceComponent parentUserInterfaceComponent)
        {
            int componentWidth = (int)Size.X;
            int componentHeight = (int)Size.Y;

            int centerX = (int)((parentSize.X / 2) - (componentWidth / 2));
            int centerY = (int)((parentSize.Y / 2) - (componentHeight / 2));

            switch (parentUserInterfaceComponent.ChildAlignment)
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
                    origin = new Vector2(parentSize.X - componentWidth, centerY);
                    break;
                case UserInterfaceAlignment.CenterDown:
                    origin = new Vector2(centerX, parentSize.Y - componentHeight);
                    break;
                case UserInterfaceAlignment.LeftUp:
                    origin = new Vector2(0, 0);
                    break;
                case UserInterfaceAlignment.RightUp:
                    origin = new Vector2(parentSize.X - componentWidth, 0);
                    break;
                case UserInterfaceAlignment.LeftDown:
                    origin = new Vector2(0, parentSize.Y - componentHeight);
                    break;
                case UserInterfaceAlignment.RightDown:
                    origin = new Vector2(parentSize.X - componentWidth, parentSize.Y - componentHeight);
                    break;
                default:
                    origin = new Vector2(Position.X, Position.Y);
                    break;
            }

            return origin;
        }

        return origin;
    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class DirectionalListUserInterfaceComponent : MultipleChildUserInterfaceComponent
{
    public ListDirection Direction = ListDirection.Horizontal;
    public int Spacing = 2;

    public DirectionalListUserInterfaceComponent(string name, ListDirection direction, Vector2 position, Vector2 size, int spacing, UserInterfaceAlignment childAlignment, List<IUserInterfaceComponent> children) : base(name, position, size, childAlignment, children)
    {
        Direction = direction;
        Spacing = spacing;
    }

    public override Vector2 GetOffsetForChild(IUserInterfaceComponent child)
    {
        float index = Children.IndexOf(child);
        Vector2 offset = Vector2.Zero;

        for (int x = 0; x < index; x++)
        {
            IUserInterfaceComponent sibling = Children[x];

            if (sibling is IMultipleChildUserInterfaceComponent multipleChildSibling)
            {
                switch (Direction)
                {
                    case ListDirection.Horizontal:
                        offset.X += multipleChildSibling.GetBoundsOfChildren().Size.X + Spacing;
                        break;
                    case ListDirection.Vertical:
                        offset.Y += multipleChildSibling.GetBoundsOfChildren().Size.Y + Spacing;
                        break;
                }
            }
            else
            {
                switch (Direction)
                {
                    case ListDirection.Horizontal:
                        offset.X += sibling.Size.X + Spacing;
                        break;
                    case ListDirection.Vertical:
                        offset.Y += sibling.Size.Y + Spacing;
                        break;
                }
            }
        }

        return offset;
    }
}

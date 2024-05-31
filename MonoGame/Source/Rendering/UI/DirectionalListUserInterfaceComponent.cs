using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class DirectionalListUserInterfaceComponent : MultipleChildUserInterfaceComponent
{
    public ListDirection Direction = ListDirection.Horizontal;
    public int Spacing = 2;

    public DirectionalListUserInterfaceComponent(string name, ListDirection direction, Vector2 localPosition, int spacing, List<IUserInterfaceComponent> children) : base(name, localPosition, children)
    {
        Direction = direction;
        Spacing = spacing;
    }

    public override Vector2 GetChildOffset(IUserInterfaceComponent child)
    {
        int index = Children.IndexOf(child);
        Vector2 offset = Vector2.Zero;

        for (int i = 0; i < index; i++)
        {
            if (Direction == ListDirection.Horizontal)
            {
                offset.X += Children[i].GetPreferredSize().X + Spacing;
            }
            else
            {
                offset.Y += Children[i].GetPreferredSize().Y + Spacing;
            }
        }

        return offset + base.GetChildOffset(child);
    }

    public override Vector2 GetPreferredSize()
    {
        float maxWidth = 0;
        float totalHeight = 0;

        foreach (var child in Children)
        {
            Vector2 childPreferredSize = child.GetPreferredSize();

            if (Direction == ListDirection.Vertical)
            {
                maxWidth = Math.Max(maxWidth, childPreferredSize.X);
                totalHeight += childPreferredSize.Y;
            }
            else
            {
                maxWidth += childPreferredSize.X;
                totalHeight = Math.Max(totalHeight, childPreferredSize.Y);
            }
        }

        if (Direction == ListDirection.Horizontal)
            maxWidth += Spacing * (Children.Count - 1);
        else
            totalHeight += Spacing * (Children.Count - 1);

        return new Vector2((int)maxWidth, (int)totalHeight);
    }
}

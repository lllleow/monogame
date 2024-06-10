using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.UI.Interfaces;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class DirectionalListUserInterfaceComponent : MultipleChildUserInterfaceComponent
{
    public DirectionalListUserInterfaceComponent(string name, ListDirection direction, Vector2 localPosition,
        int spacing, List<IUserInterfaceComponent> children) : base(name, localPosition, children)
    {
        Direction = direction;
        Spacing = spacing;
    }

    public ListDirection Direction { get; set; } = ListDirection.Horizontal;
    public int Spacing { get; set; } = 2;

    public override Vector2 GetChildOffset(IUserInterfaceComponent child)
    {
        var index = Children.IndexOf(child);
        var offset = Vector2.Zero;

        for (var i = 0; i < index; i++)
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
            var childPreferredSize = child.GetPreferredSize();

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
        {
            maxWidth += Spacing * (Children.Count - 1);
        }
        else
        {
            totalHeight += Spacing * (Children.Count - 1);
        }

        return new Vector2((int)maxWidth, (int)totalHeight);
    }
}
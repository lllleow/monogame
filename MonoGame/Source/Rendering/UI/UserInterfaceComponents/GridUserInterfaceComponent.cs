using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGame;

public class GridUserInterfaceComponent : MultipleChildUserInterfaceComponent
{
    public int MaxColumns { get; set; }
    public int MaxRows { get; set; }
    public Vector2 Spacing = Vector2.Zero;

    public GridUserInterfaceComponent(string name, int maxColumns, int maxRows, Vector2 spacing, Vector2 localPosition, List<IUserInterfaceComponent> children) : base(name, localPosition, children)
    {
        MaxColumns = maxColumns;
        MaxRows = maxRows;
        Spacing = spacing;
    }

    public override Vector2 GetPreferredSize()
    {
        return base.GetPreferredSize();
    }

    public override Vector2 GetChildOffset(IUserInterfaceComponent child)
    {
        int index = Children.IndexOf(child);

        int row = index / MaxColumns;
        int column = index % MaxColumns;

        Vector2 preferredSize = child.GetPreferredSize();

        return new Vector2(column * (preferredSize.X + Spacing.X), row * (preferredSize.Y + Spacing.Y));
    }
}

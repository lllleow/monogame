using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGame;

public class GridUserInterfaceComponent : MultipleChildUserInterfaceComponent
{
    public int Columns { get; set; }
    public int Rows { get; set; }
    public Vector2 Spacing = Vector2.Zero;

    public GridUserInterfaceComponent(string name, int columns, int rows, Vector2 spacing, Vector2 position, Vector2 size, UserInterfaceAlignment childAlignment, List<IUserInterfaceComponent> children) : base(name, position, size, childAlignment, children)
    {
        Columns = columns;
        Rows = rows;
        Spacing = spacing;
    }

    public override Vector2 GetOffsetForChild(IUserInterfaceComponent child)
    {
        int index = Children.IndexOf(child);

        int column = index % Columns;
        int row = index / Columns;
        
        Vector2 size = child.Size;
        return new Vector2((column * size.X) + (column * Spacing.X), (row * size.Y) + (row * Spacing.Y));
    }
}

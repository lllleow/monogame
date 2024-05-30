using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGame;

public class GridUserInterfaceComponent : MultipleChildUserInterfaceComponent
{
    public int Columns { get; set; }
    public int Rows { get; set; }
    public Vector2 Spacing = Vector2.Zero;

    public GridUserInterfaceComponent(string name, int columns, int rows, Vector2 spacing, Vector2 localPosition, List<IUserInterfaceComponent> children) : base(name, localPosition, children)
    {
        Columns = columns;
        Rows = rows;
        Spacing = spacing;
    }
}

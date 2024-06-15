using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.UI.Interfaces;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class GridUserInterfaceComponent : MultipleChildUserInterfaceComponent
{
    public GridUserInterfaceComponent(int maxColumns, Vector2 spacing, List<IUserInterfaceComponent> children) : base("grid", new Vector2(0, 0), children)
    {
        MaxColumns = maxColumns;
        Spacing = spacing;
    }

    public int MaxColumns { get; set; }
    public Vector2 Spacing { get; set; } = Vector2.Zero;

    public override Vector2 GetPreferredSize()
    {
        Vector2 childSize = Children.First().GetPreferredSize();
        int rows = Children.Count / MaxColumns;
        return new Vector2((childSize.X * MaxColumns) + (Spacing.X * (MaxColumns - 1)), (childSize.Y * rows) + (Spacing.Y * (rows - 1)));
    }

    public override Vector2 GetChildOffset(IUserInterfaceComponent child)
    {
        var index = Children.IndexOf(child);

        var row = index / MaxColumns;
        var column = index % MaxColumns;

        var preferredSize = child.GetPreferredSize();

        return new Vector2(column * (preferredSize.X + Spacing.X), row * (preferredSize.Y + Spacing.Y));
    }
}
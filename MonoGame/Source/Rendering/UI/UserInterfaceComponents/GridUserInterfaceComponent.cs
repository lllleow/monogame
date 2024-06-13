using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Source.Rendering.UI.Interfaces;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class GridUserInterfaceComponent : MultipleChildUserInterfaceComponent
{
    public GridUserInterfaceComponent(string name, int maxColumns, int maxRows, Vector2 spacing, Vector2 localPosition, List<IUserInterfaceComponent> children) : base(name, localPosition, children)
    {
        MaxColumns = maxColumns;
        MaxRows = maxRows;
        Spacing = spacing;
    }

    public int MaxColumns { get; set; }
    public int MaxRows { get; set; }
    public Vector2 Spacing { get; set; } = Vector2.Zero;

    public override Vector2 GetPreferredSize()
    {
        return base.GetPreferredSize();
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
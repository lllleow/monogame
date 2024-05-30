using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class DirectionalListUserInterfaceComponent : GridUserInterfaceComponent
{
    public ListDirection Direction = ListDirection.Horizontal;

    public DirectionalListUserInterfaceComponent(string name, ListDirection direction, int spacing, Vector2 position, Vector2 size, UserInterfaceAlignment childAlignment, List<IUserInterfaceComponent> children) : base(name, 0, 0, position, size, childAlignment, children)
    {
        Direction = direction;

        switch (Direction)
        {
            case ListDirection.Horizontal:
                Columns = children.Count;
                Rows = 1;
                Spacing = new Vector2(spacing, 0);
                break;
            case ListDirection.Vertical:
                Columns = 1;
                Rows = children.Count;
                Spacing = new Vector2(0, spacing);
                break;
        }
    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class DirectionalListUserInterfaceComponent : MultipleChildUserInterfaceComponent
{
    public ListDirection Direction = ListDirection.Horizontal;
    public int Spacing = 2;

    public DirectionalListUserInterfaceComponent(string name, ListDirection direction, Vector2 position, Vector2 size, List<IUserInterfaceComponent> children) : base(name, position, size, children)
    {
        Direction = direction;
    }

    public override Vector2 GetOffsetForChild(IUserInterfaceComponent child)
    {
        float index = Children.IndexOf(child);
        switch (Direction)
        {
            case ListDirection.Horizontal:
                return new Vector2((index * child.Size.X) + (index * Spacing), 0);
            case ListDirection.Vertical:
                return new Vector2(0, (index * child.Size.Y) + (index * Spacing));
            default:
                return Vector2.Zero;
        }
    }
}

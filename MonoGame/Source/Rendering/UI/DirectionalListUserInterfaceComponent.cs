using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class DirectionalListUserInterfaceComponent : MultipleChildUserInterfaceComponent
{
    public ListDirection Direction = ListDirection.Horizontal;
    public int Spacing = 2;

    public DirectionalListUserInterfaceComponent(string name, ListDirection direction, Vector2 position, Vector2 size, int spacing, List<IUserInterfaceComponent> children) : base(name, position, size, children)
    {
        Direction = direction;
        Spacing = spacing;
    }
}

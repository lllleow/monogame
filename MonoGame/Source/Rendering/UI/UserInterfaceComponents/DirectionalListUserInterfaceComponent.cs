using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class DirectionalListUserInterfaceComponent : UserInterfaceComponent
{
    public ListDirection Direction = ListDirection.Horizontal;
    public Vector2 Padding = new Vector2(2, 2);

    public DirectionalListUserInterfaceComponent(string name, ListDirection direction, Rectangle? bounds, List<IUserInterfaceComponent> childComponents) : base(name, bounds, childComponents)
    {
        Direction = direction;
        UpdateChildBounds();
    }

    public void UpdateChildBounds()
    {
        foreach (IUserInterfaceComponent component in ChildComponents)
        {
            switch (Direction)
            {
                case ListDirection.Horizontal:
                    component.UpdatePosition(ChildComponents.IndexOf(component) * new Vector2(component.GetBounds().Width + Padding.X, 0));
                    break;
                case ListDirection.Vertical:
                    component.UpdatePosition(ChildComponents.IndexOf(component) * new Vector2(0, component.GetBounds().Height + Padding.Y));
                    break;
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}

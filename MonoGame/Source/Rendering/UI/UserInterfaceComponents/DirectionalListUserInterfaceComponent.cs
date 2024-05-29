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
    }

    public override void InitializeComponent()
    {
        UpdateChildComponentsPositions();
        UpdateAlignment();
    }

    public override void UpdateChildComponentsPositions()
    {
        foreach (IUserInterfaceComponent component in ChildComponents)
        {
            var index = ChildComponents.IndexOf(component);

            switch (Direction)
            {
                case ListDirection.Horizontal:
                    component.UpdatePosition(index * new Vector2(component.Bounds?.Width ?? 0 + Padding.X, 0));
                    break;
                case ListDirection.Vertical:
                    component.UpdatePosition(index * new Vector2(0, component.Bounds?.Height ?? 0 + Padding.Y));
                    break;
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
    }
}

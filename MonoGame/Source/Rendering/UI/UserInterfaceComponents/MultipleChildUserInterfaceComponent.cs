using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Rendering.UI.Interfaces;
using MonoGame.Source.Utils.Helpers;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class MultipleChildUserInterfaceComponent : UserInterfaceComponent
{
    public MultipleChildUserInterfaceComponent(string name, Vector2 localPosition, List<IUserInterfaceComponent> children) : base(name, localPosition)
    {
        Children = children;

        foreach (var child in Children)
        {
            child.Initialize(this);
        }
    }

    public List<IUserInterfaceComponent> Children { get; set; }

    public void AddChild(IUserInterfaceComponent child)
    {
        child?.Initialize(this);
        if (child != null)
        {
            Children.Add(child);
        }
    }

    public void RemoveChild(IUserInterfaceComponent child)
    {
        child.Dispose();
        _ = Children.Remove(child);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        foreach (var child in Children)
        {
            child.Draw(spriteBatch);
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        foreach (var child in Children)
        {
            child.Update(gameTime);
        }
    }

    public override Vector2 GetPreferredSize()
    {
        var rectangles = new Rectangle[Children.Count];
        for (var i = 0; i < Children.Count; i++)
        {
            var size = Children[i].GetPreferredSize();
            rectangles[i] = new Rectangle(
                (int)Children[i].GetPositionRelativeToParent().X,
                (int)Children[i].GetPositionRelativeToParent().Y,
                (int)size.X,
                (int)size.Y);
        }

        var minimumBoundingRectangle = RectangleHelper.GetMinimumBoundingRectangle(rectangles);
        Vector2 endSize = new Vector2(minimumBoundingRectangle.Width, minimumBoundingRectangle.Height);
        CalculatedSize = endSize;
        return endSize;
    }

    public void RemoveAllChildren()
    {
        Children.Clear();
    }

    public void AddManyChildren(List<IUserInterfaceComponent> children)
    {
        foreach (var child in children)
        {
            AddChild(child);
        }
    }

    public void ReplaceChildren(List<IUserInterfaceComponent> children)
    {
        RemoveAllChildren();
        AddManyChildren(children);
    }

    public override void OnEnabledChanged()
    {
        base.OnEnabledChanged();
        foreach (var child in Children)
        {
            child.Enabled = Enabled;
        }
    }
}
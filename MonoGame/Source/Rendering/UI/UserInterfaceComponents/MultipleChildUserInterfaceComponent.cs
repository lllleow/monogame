
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Rendering.UI.Interfaces;

namespace MonoGame.Source.Rendering.UI.UserInterfaceComponents;

public class MultipleChildUserInterfaceComponent : UserInterfaceComponent
{
    public List<IUserInterfaceComponent> Children { get; set; }
    private RectangleHelper rectangleHelper = new RectangleHelper();

    public MultipleChildUserInterfaceComponent(string name, Vector2 localPosition, List<IUserInterfaceComponent> children) : base(name, localPosition)
    {
        Children = children;

        foreach (var child in Children)
        {
            child.Initialize(this);
        }
    }

    public void AddChild(IUserInterfaceComponent child)
    {
        child.Initialize(this);
        Children.Add(child);
    }

    public void RemoveChild(IUserInterfaceComponent child)
    {
        Children.Remove(child);
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
        Rectangle[] rectangles = new Rectangle[Children.Count];
        for (int i = 0; i < Children.Count; i++)
        {
            Vector2 size = Children[i].GetPreferredSize();
            rectangles[i] = new Rectangle((int)Children[i].GetPositionRelativeToParent().X, (int)Children[i].GetPositionRelativeToParent().Y, (int)size.X, (int)size.Y);
        }

        Rectangle minimumBoundingRectangle = rectangleHelper.GetMinimumBoundingRectangle(rectangles);
        return new Vector2(minimumBoundingRectangle.Width, minimumBoundingRectangle.Height);
    }
}

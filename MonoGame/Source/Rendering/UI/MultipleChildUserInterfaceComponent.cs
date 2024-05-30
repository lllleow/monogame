using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class MultipleChildUserInterfaceComponent : ParentUserInterfaceComponent, IMultipleChildUserInterfaceComponent
{
    public List<IUserInterfaceComponent> Children { get; set; }

    public MultipleChildUserInterfaceComponent(string name, Vector2 position, Vector2 size, UserInterfaceAlignment childAlignment, List<IUserInterfaceComponent> children) : base(name, position, size, childAlignment)
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

    public Rectangle GetBoundsOfChildren(List<IUserInterfaceComponent> excluding = null)
    {
        if (Children.Count == 0)
        {
            return Rectangle.Empty;
        }

        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int maxX = int.MinValue;
        int maxY = int.MinValue;

        List<IUserInterfaceComponent> children = new List<IUserInterfaceComponent>(Children);
        if (excluding != null)
        {
            foreach (var child in excluding)
            {
                children.Remove(child);
            }
        }

        foreach (var child in children)
        {
            Vector2 childPosition = GetOffsetForChild(child);
            Rectangle childBounds = new Rectangle((int)childPosition.X, (int)childPosition.Y, (int)child.Size.X, (int)child.Size.Y);
            minX = Math.Min(minX, childBounds.Left);
            minY = Math.Min(minY, childBounds.Top);
            maxX = Math.Max(maxX, childBounds.Right);
            maxY = Math.Max(maxY, childBounds.Bottom);
        }

        return new Rectangle(minX, minY, maxX - minX, maxY - minY);
    }

    public virtual Vector2 GetOffsetForChild(IUserInterfaceComponent child)
    {
        return Vector2.Zero;
    }
}

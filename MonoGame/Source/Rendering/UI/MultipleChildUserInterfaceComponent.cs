using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class MultipleChildUserInterfaceComponent : UserInterfaceComponent, IMultipleChildUserInterfaceComponent
{
    public List<IUserInterfaceComponent> Children { get; set; }

    public MultipleChildUserInterfaceComponent(string name, Vector2 position, Vector2 size, List<IUserInterfaceComponent> children) : base(name, position, size)
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

    public virtual Vector2 GetOffsetForChild(IUserInterfaceComponent child)
    {
        return Vector2.Zero;
    }
}

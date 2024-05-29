using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class UserInterfaceComponent : IUserInterfaceComponent
{
    public string Name { get; set; }
    public Rectangle? Bounds { get; set; }
    public Action<IUserInterfaceComponent> CallbackFunction { get; set; }
    public List<IUserInterfaceComponent> ChildComponents { get; set; } = new List<IUserInterfaceComponent>();

    public UserInterfaceComponent(string name, Rectangle? bounds)
    {
        Name = name;
        Bounds = bounds;
    }

    public UserInterfaceComponent(string name, Rectangle? bounds, List<IUserInterfaceComponent> childComponents)
    {
        Name = name;
        Bounds = bounds;
        ChildComponents = childComponents;
    }

    public void SetCallbackFunction(Action<IUserInterfaceComponent> callbackFunction)
    {
        CallbackFunction = callbackFunction;
    }

    public void UpdateBounds(Rectangle bounds)
    {
        Bounds = bounds;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (IUserInterfaceComponent childComponent in ChildComponents)
        {
            childComponent.Draw(spriteBatch);
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        foreach (IUserInterfaceComponent childComponent in ChildComponents)
        {
            childComponent.Update(gameTime);
        }
    }

    public void UpdatePosition(Vector2 position)
    {
        Rectangle bounds = GetBounds();
        bounds.Location = position.ToPoint();

        UpdateBounds(bounds);
    }

    public void UpdateSize(Vector2 size)
    {
        Rectangle bounds = GetBounds();
        bounds.Size = size.ToPoint();

        UpdateBounds(bounds);
    }

    public Rectangle GetBounds()
    {
        Rectangle bounds = Bounds ?? Rectangle.Empty;
        Vector2 position = bounds.Location.ToVector2();
        Vector2 size = bounds.Size.ToVector2();

        foreach (IUserInterfaceComponent childComponent in ChildComponents)
        {
            Rectangle childBounds = childComponent.GetBounds();
            position = Vector2.Min(position, childBounds.Location.ToVector2());
            size = Vector2.Max(size, childBounds.Size.ToVector2());
        }

        return new Rectangle(position.ToPoint(), size.ToPoint());
    }
}
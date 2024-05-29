using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class UserInterfaceComponent : IUserInterfaceComponent
{
    public string Name { get; set; }
    public Rectangle? Bounds { get; set; }
    public Action<IUserInterfaceComponent> CallbackFunction { get; set; }
    public List<IUserInterfaceComponent> ChildComponents { get; set; } = new List<IUserInterfaceComponent>();
    public UserInterfaceAlignment userInterfaceAlignment { get; set; } = UserInterfaceAlignment.CenterDown;
    public IUserInterfaceComponent ParentComponent { get; set; }

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

        foreach (IUserInterfaceComponent childComponent in ChildComponents)
        {
            childComponent.ParentComponent = this;
        }
    }

    public void SetCallbackFunction(Action<IUserInterfaceComponent> callbackFunction)
    {
        CallbackFunction = callbackFunction;
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
        bounds.Location = ParentComponent != null ? ParentComponent.GetBounds().Location + position.ToPoint() : position.ToPoint();

        Bounds = bounds;
    }

    public void UpdateSize(Vector2 size)
    {
        Rectangle bounds = GetBounds();
        bounds.Size = size.ToPoint();

        Bounds = bounds;
    }

    public Rectangle GetBounds()
    {
        if (ChildComponents.Count == 0)
            return Bounds ?? Rectangle.Empty;

        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int maxX = int.MinValue;
        int maxY = int.MinValue;

        foreach (IUserInterfaceComponent childComponent in ChildComponents)
        {
            Rectangle childBounds = childComponent.GetBounds();

            minX = Math.Min(minX, childBounds.Left);
            minY = Math.Min(minY, childBounds.Top);
            maxX = Math.Max(maxX, childBounds.Right);
            maxY = Math.Max(maxY, childBounds.Bottom);
        }

        Rectangle bounds = new Rectangle(minX, minY, maxX - minX, maxY - minY);
        bounds.Location = Bounds?.Location + bounds.Location ?? bounds.Location;

        return bounds;
    }

    public virtual void UpdateAlignment()
    {
        Rectangle screenRectangle = new Rectangle(0, 0, Globals.graphicsDevice.GraphicsDevice.Viewport.Width, Globals.graphicsDevice.GraphicsDevice.Viewport.Height);
        screenRectangle = TransformRectangle.Transform(screenRectangle, Matrix.Invert(Globals.userInterfaceHandler.Transform));

        Rectangle bounds = GetBounds();
        int componentWidth = bounds.Width;
        int componentHeight = bounds.Height;

        int centerX = (screenRectangle.Width / 2) - (componentWidth / 2);
        int centerY = (screenRectangle.Height / 2) - (componentHeight / 2);

        switch (userInterfaceAlignment)
        {
            case UserInterfaceAlignment.LeftCenter:
                Bounds = new Rectangle(0, centerY, bounds.Width, bounds.Height);
                break;
            case UserInterfaceAlignment.CenterUp:
                Bounds = new Rectangle(centerX, 0, bounds.Width, bounds.Height);
                break;
            case UserInterfaceAlignment.Center:
                Bounds = new Rectangle(centerX, centerY, bounds.Width, bounds.Height);
                break;
            case UserInterfaceAlignment.RightCenter:
                Bounds = new Rectangle(screenRectangle.Width - componentWidth, centerY, bounds.Width, bounds.Height);
                break;
            case UserInterfaceAlignment.CenterDown:
                Bounds = new Rectangle(centerX, screenRectangle.Height - componentHeight, bounds.Width, bounds.Height);
                break;
            case UserInterfaceAlignment.LeftUp:
                Bounds = new Rectangle(0, 0, bounds.Width, bounds.Height);
                break;
            case UserInterfaceAlignment.RightUp:
                Bounds = new Rectangle(screenRectangle.Width - componentWidth, 0, bounds.Width, bounds.Height);
                break;
            case UserInterfaceAlignment.LeftDown:
                Bounds = new Rectangle(0, screenRectangle.Height - componentHeight, bounds.Width, bounds.Height);
                break;
            case UserInterfaceAlignment.RightDown:
                Bounds = new Rectangle(screenRectangle.Width - componentWidth, screenRectangle.Height - componentHeight, bounds.Width, bounds.Height);
                break;
            default:
                Bounds = new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height);
                break;
        }
    }
    public virtual void InitializeComponent()
    {
        throw new NotImplementedException();
    }

    public virtual void UpdateChildComponentsPositions()
    {
        throw new NotImplementedException();
    }

}

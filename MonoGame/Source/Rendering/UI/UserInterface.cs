using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class UserInterface : IUserInterface
{
    public string Name { get; set; }
    public List<IUserInterfaceComponent> Components { get; set; } = new List<IUserInterfaceComponent>();
    public Action<IUserInterfaceComponent> OnComponentChanged { get; set; } = (component) => { };
    public bool Visible { get; set; } = true;

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (IUserInterfaceComponent component in Components)
        {
            foreach (IUserInterfaceComponent childComponent in component.ChildComponents)
            {
                component.Draw(spriteBatch);
            }
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        foreach (IUserInterfaceComponent component in Components)
        {
            foreach (IUserInterfaceComponent childComponent in component.ChildComponents)
            {
                component.Update(gameTime);
            }
        }
    }

    public void AddComponent(IUserInterfaceComponent component)
    {
        component.SetCallbackFunction(OnComponentChanged);
        Components.Add(component);
    }

    public void RemoveComponent(IUserInterfaceComponent component)
    {
        Components.Remove(component);
    }
}

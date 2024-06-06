
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Rendering.UI.Interfaces;

namespace MonoGame;

public class UserInterface : IUserInterface
{
    public string Name { get; set; }
    public List<IUserInterfaceComponent> Components { get; set; } = new List<IUserInterfaceComponent>();
    public bool Visible { get; set; } = true;

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (IUserInterfaceComponent component in Components)
        {
            component.Draw(spriteBatch);
        }
    }

    public virtual void Update(GameTime gameTime)
    {
        foreach (IUserInterfaceComponent component in Components)
        {
            component.Update(gameTime);
        }
    }

    public void AddComponent(IUserInterfaceComponent component)
    {
        component.Initialize(null);
        Components.Add(component);
    }

    public void RemoveComponent(IUserInterfaceComponent component)
    {
        Components.Remove(component);
    }
}

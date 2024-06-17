using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Rendering.UI.Interfaces;

namespace MonoGame.Source.Rendering.UI;

public class UserInterface : IUserInterface
{
    public string Name { get; set; }
    public List<IUserInterfaceComponent> Components { get; set; } = [];

    private bool _enabled = true;

    public bool Enabled
    {
        get
        {
            return _enabled;
        }
        set
        {
            if (_enabled != value)
            {
                _enabled = value;
                OnEnabledChanged();
            }
        }
    }

    protected virtual void OnEnabledChanged()
    {
        foreach (var component in Components)
        {
            component.Enabled = Enabled;
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (!Enabled) return;
        foreach (var component in Components)
        {
            Globals.SpriteBatch.End();
            Globals.DefaultSpriteBatchUIBegin();
            component.Draw(spriteBatch);
        }
    }

    public virtual bool IsVisible()
    {
        return true;
    }

    public virtual void Initialize()
    {
    }

    public virtual void Update(GameTime gameTime)
    {
        if (!Enabled) return;
        foreach (var component in Components)
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
        _ = Components.Remove(component);
    }
}
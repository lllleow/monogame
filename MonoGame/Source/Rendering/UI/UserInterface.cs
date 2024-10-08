﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Rendering.UI.Interfaces;

namespace MonoGame.Source.Rendering.UI;

public class UserInterface : IUserInterface
{
    public string Name { get; set; }
    public List<IUserInterfaceComponent> Components { get; set; } = [];

    public virtual void Draw(SpriteBatch spriteBatch)
    {
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
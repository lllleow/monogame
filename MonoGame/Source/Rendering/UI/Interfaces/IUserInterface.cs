using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Source.Rendering.UI.Interfaces;

public interface IUserInterface
{
    public string Name { get; set; }
    public bool Visible { get; set; }
    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract void Update(GameTime gameTime);
    public List<IUserInterfaceComponent> Components { get; set; }
    public abstract void AddComponent(IUserInterfaceComponent component);
    public abstract void RemoveComponent(IUserInterfaceComponent component);
}

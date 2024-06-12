using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MonoGame.Source.Rendering.UI.Interfaces;

public interface IUserInterface
{
    public string Name { get; set; }
    public bool Visible { get; set; }
    public List<IUserInterfaceComponent> Components { get; set; }
    public void Draw(SpriteBatch spriteBatch);
    public void Update(GameTime gameTime);
    public void AddComponent(IUserInterfaceComponent component);
    public void RemoveComponent(IUserInterfaceComponent component);
}
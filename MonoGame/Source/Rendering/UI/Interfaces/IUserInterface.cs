using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Source.Rendering.UI.Interfaces;

public interface IUserInterface
{
    public string Name { get; set; }
    public List<IUserInterfaceComponent> Components { get; set; }
    public void Draw(SpriteBatch spriteBatch);
    public void Update(GameTime gameTime);
    public void AddComponent(IUserInterfaceComponent component);
    public void RemoveComponent(IUserInterfaceComponent component);
    public bool IsVisible();
    public void Initialize();
}
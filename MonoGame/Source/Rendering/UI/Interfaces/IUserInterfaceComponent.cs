using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Source.Rendering.UI.Interfaces;

public interface IUserInterfaceComponent
{
    public string Name { get; set; }
    public Vector2 LocalPosition { get; set; }
    public abstract void Initialize(IUserInterfaceComponent parent);
    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract void Update(GameTime gameTime);
    public abstract Vector2 GetPositionRelativeToParent();
    public abstract Vector2 GetPreferredSize();
    public abstract Vector2 GetChildOffset(IUserInterfaceComponent child);
}

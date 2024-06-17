using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Source.Rendering.UI.Interfaces;

public interface IUserInterfaceComponent
{
    public string Name { get; set; }
    public List<InputSubscriberReference> Subscribers { get; set; }
    public Vector2 LocalPosition { get; set; }
    public Vector2 CalculatedSize { get; set; }
    public void Initialize(IUserInterfaceComponent parent);
    public void Draw(SpriteBatch spriteBatch);
    public void Update(GameTime gameTime);
    public Vector2 GetPositionRelativeToParent();
    public Vector2 GetPreferredSize();
    public Vector2 GetChildOffset(IUserInterfaceComponent child);
    public bool Enabled { get; set; }
    public int GetPercentageOfScreenWidth(float percent);
    public int GetPercentageOfScreenHeight(float percent);
    public void OnEnabledChanged();
    public void Dispose();
    public void Build();
}
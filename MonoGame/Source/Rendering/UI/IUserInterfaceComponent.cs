using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public interface IUserInterfaceComponent : IChildUserInterfaceComponent
{
    public string Name { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Size { get; set; }
    public Vector2 ContentPadding { get; set; }
    public Vector2 GetRelativePosition();
    public Vector2 GetRelativeContentPadding();
    public abstract void Initialize(IUserInterfaceComponent parent);
    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract void Update(GameTime gameTime);
}

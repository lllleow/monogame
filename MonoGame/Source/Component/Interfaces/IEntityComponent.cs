using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public interface IEntityComponent
{
    IGameEntity Entity { get; set; }
    void Initialize();
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}

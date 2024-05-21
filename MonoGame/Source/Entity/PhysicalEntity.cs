using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public abstract class PhysicalEntity : GameEntity
{
    public abstract void Draw(SpriteBatch spriteBatch);
}

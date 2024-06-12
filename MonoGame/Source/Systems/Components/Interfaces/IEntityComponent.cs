using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Entity.Interfaces;
using System;

namespace MonoGame.Source.Systems.Components.Interfaces;

public interface IEntityComponent
{
    IGameEntity Entity { get; set; }
    bool Initialized { get; set; }
    void Initialize();
    public void SetEntity(IGameEntity entity);
    public void Update(GameTime gameTime);
    public void Draw(SpriteBatch spriteBatch);
    public Type GetComponentStateType();
}
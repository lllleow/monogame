using System;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public abstract class EntityComponent : IEntityComponent
{
    private IGameEntity _entity;
    public IGameEntity Entity
    {
        get => _entity;
        set => _entity = value;
    }

    public void BaseInitialize(IGameEntity entity)
    {
        this.Entity = entity;
    }

    public abstract void Draw(SpriteBatch spriteBatch);

    public abstract void Initialize();

    public abstract void Update(Microsoft.Xna.Framework.GameTime gameTime);
}

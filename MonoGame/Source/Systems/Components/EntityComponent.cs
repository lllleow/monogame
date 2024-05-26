using System;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Components.Interfaces;
using MonoGame.Source.Systems.Entity.Interfaces;

namespace MonoGame.Source.Systems.Components;

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
        Entity = entity;
    }

    public abstract void Draw(SpriteBatch spriteBatch);

    public abstract void Initialize();

    public abstract void Update(Microsoft.Xna.Framework.GameTime gameTime);
}

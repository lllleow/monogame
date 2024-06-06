using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Components.Interfaces;

namespace MonoGame.Source.Systems.Components;

public abstract class EntityComponent : IEntityComponent
{
    private IGameEntity _entity;

    public IGameEntity Entity
    {
        get => _entity;
        set => _entity = value;
    }

    public bool Initialized { get; set; } = false;

    public void BaseInitialize(IGameEntity entity)
    {
        Entity = entity;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
    }

    public virtual void Initialize()
    {
    }

    public void SetEntity(IGameEntity entity)
    {
        Entity = entity;
    }

    public virtual void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
    }
}

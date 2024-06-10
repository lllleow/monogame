using System;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Multiplayer;
using MonoGame.Source.Systems.Components.Interfaces;
using MonoGame.Source.Systems.Entity.Interfaces;
using MonoGame_Common.Messages.Components;

namespace MonoGame.Source.Systems.Components;

public abstract class EntityComponent : IEntityComponent
{
    public IGameEntity Entity { get; set; }

    public bool Initialized { get; set; } = false;

    public virtual void Draw(SpriteBatch spriteBatch)
    {
    }

    public virtual Type GetComponentStateType()
    {
        return null;
    }

    public virtual void Initialize()
    {
        Type componentStateType = GetComponentStateType();
        if (componentStateType != null)
        {
            NetworkClient.SendMessage(new RegisterEntityComponentNetworkMessage(Entity.UUID, componentStateType));
        }
    }

    public void SetEntity(IGameEntity entity)
    {
        Entity = entity;
    }

    public virtual void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
    }
}

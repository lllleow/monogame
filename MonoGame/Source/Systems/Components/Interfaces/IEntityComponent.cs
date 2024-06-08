﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Entity.Interfaces;

namespace MonoGame.Source.Systems.Components.Interfaces;

public interface IEntityComponent
{
    IGameEntity Entity { get; set; }

    bool Initialized { get; set; }

    abstract void Initialize();

    public abstract void SetEntity(IGameEntity entity);

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(SpriteBatch spriteBatch);
    public Type GetComponentStateType();
}

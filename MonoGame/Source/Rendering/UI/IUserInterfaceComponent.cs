﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public interface IUserInterfaceComponent
{
    public string Name { get; set; }
    public Vector2 LocalPosition { get; set; }
    public Vector2 Size { get; set; }
    public abstract void Initialize(IUserInterfaceComponent parent);
    public abstract void Draw(SpriteBatch spriteBatch, Vector2 origin);
    public abstract void Update(GameTime gameTime);
    public abstract Vector2 GetPositionRelativeToParent();
}

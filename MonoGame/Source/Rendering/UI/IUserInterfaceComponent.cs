﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public interface IUserInterfaceComponent
{
    public string Name { get; set; }
    public Rectangle? Bounds { get; set; }
    public List<IUserInterfaceComponent> ChildComponents { get; set; }
    public Action<IUserInterfaceComponent> CallbackFunction { get; set; }
    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract void Update(GameTime gameTime);
    public abstract void SetCallbackFunction(Action<IUserInterfaceComponent> callbackFunction);
    public abstract void UpdateBounds(Rectangle bounds);
    public abstract void UpdatePosition(Vector2 position);
    public abstract void UpdateSize(Vector2 size);
    public abstract Rectangle GetBounds();
}

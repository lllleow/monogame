using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public interface IGameEntity
{
    List<IEntityComponent> components { get; set; }
    Vector2 Position { get; set; }
    Vector2 Speed { get; set; }
    void Update(GameTime gameTime);
    void BaseUpdate(GameTime gameTime);
    void AddComponent(IEntityComponent component);
    void RemoveComponent(IEntityComponent component);
    T GetFirstComponent<T>();
    List<T> GetComponents<T>();
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Components.Interfaces;

namespace MonoGame;

public interface IGameEntity
{

    List<IEntityComponent> components { get; set; }

    string UUID { get; set; }

    Vector2 Position { get; set; }

    Vector2 Speed { get; set; }

    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch);

    void AddComponent(IEntityComponent component);

    void RemoveComponent(IEntityComponent component);

    T GetFirstComponent<T>();

    List<T> GetComponents<T>();

    bool ContainsComponent<T>();
}

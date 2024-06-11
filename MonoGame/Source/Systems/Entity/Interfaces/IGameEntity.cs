using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Components.Interfaces;
using System.Collections.Generic;

namespace MonoGame.Source.Systems.Entity.Interfaces;

public interface IGameEntity
{
    List<IEntityComponent> Components { get; set; }

    string UUID { get; set; }

    Vector2 Position { get; set; }

    void Update(GameTime gameTime);

    void Draw(SpriteBatch spriteBatch);

    void AddComponent(IEntityComponent component);

    void RemoveComponent(IEntityComponent component);

    T GetFirstComponent<T>();

    List<T> GetComponents<T>();

    bool ContainsComponent<T>();

    public Rectangle GetEntityBoundsAtPosition(Vector2 position);
}
using System;
using Microsoft.Xna.Framework;

namespace MonoGame;

public class EntityState
{
    public Vector2 Position { get; set; }

    public EntityState()
    {

    }

    public EntityState(IGameEntity entity)
    {
        Position = entity.Position;
    }
}

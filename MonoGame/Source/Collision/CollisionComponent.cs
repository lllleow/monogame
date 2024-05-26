using System;

namespace MonoGame;

public class CollisionComponent
{
    public IGameEntity Entity { get; set; }

    public CollisionComponent(IGameEntity entity)
    {
        Entity = entity;
    }
}

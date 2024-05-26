using System;

namespace MonoGame;

public class CollisionComponent
{
    public IGameEntity Entity { get; set; }
    public bool IsColliding { get; set; }

    public CollisionComponent(IGameEntity entity)
    {
        Entity = entity;
    }

    public void Update()
    {
        IsColliding = false;
    }

    public void OnCollision(IGameEntity otherEntity)
    {
        IsColliding = true;
    }
}

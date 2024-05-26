using System;
using MonoGame.Source.Systems.Entity.Interfaces;

namespace MonoGame.Source.Systems.Components.Collision;

public class CollisionComponent
{
    public IGameEntity Entity { get; set; }

    public CollisionComponent(IGameEntity entity)
    {
        Entity = entity;
    }
}

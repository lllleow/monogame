using System;
using LiteNetLib.Utils;
using MonoGame.Source.States.Components;
using MonoGame.Source.Systems.Components.Collision;
using MonoGame.Source.Systems.Components.Collision.Enum;

namespace MonoGame;

public class CollisionComponentState : ComponentState
{
    public CollisionMode Mode { get; set; } = CollisionMode.BoundingBox;

    public CollisionComponentState(CollisionComponent collisionComponent)
    {
        Mode = collisionComponent.Mode;
    }

    public CollisionComponentState()
    {
    }
}

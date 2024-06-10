using System;
using MonoGame_Common.Enums;
using MonoGame_Common.States.Components;
using MonoGame.Source.Systems.Components.BoundingBox;
using MonoGame.Source.Systems.Components.Collision.Controller;

namespace MonoGame.Source.Systems.Components.Collision;

public class CollisionComponent : EntityComponent
{
    public CollisionComponent(CollisionMode mode)
    {
        Mode = mode;
    }

    public CollisionMode Mode { get; set; }
    public CollisionComponentNetworkController NetworkController { get; set; }

    public override void Initialize()
    {
        base.Initialize();
        if (Mode == CollisionMode.BoundingBox && !Entity.ContainsComponent<BoundingBoxComponent>())
            throw new Exception(
                "CollisionComponent in BoundingBox mode requires a BoundingBoxComponent to be present on the entity.");

        NetworkController = new CollisionComponentNetworkController();
        NetworkController.SetCollisionMode(this);
    }

    public override Type GetComponentStateType()
    {
        return typeof(CollisionComponentState);
    }
}
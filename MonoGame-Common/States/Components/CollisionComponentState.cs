using MonoGame_Common.Enums;

namespace MonoGame_Common.States.Components;

public class CollisionComponentState : ComponentState
{
    public CollisionComponentState(CollisionMode collisionMode)
    {
        Mode = collisionMode;
    }

    public CollisionComponentState()
    {
    }

    public CollisionMode Mode { get; set; } = CollisionMode.BoundingBox;
}
using System;
using LiteNetLib.Utils;
using MonoGame.Source.States.Components;
using MonoGame.Source.Systems.Components.Collision.Enum;

namespace MonoGame;

public class CollisionComponentState : ComponentState
{
    public CollisionMode Mode { get; set; } = CollisionMode.BoundingBox;

    public CollisionComponentState()
    {
    }

    public override void Serialize(NetDataWriter writer)
    {
        writer.Put((int)Mode);
    }

    public override void Deserialize(NetDataReader reader)
    {
        Mode = (CollisionMode)reader.GetInt();
    }
}

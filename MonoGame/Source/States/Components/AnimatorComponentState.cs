using LiteNetLib.Utils;

namespace MonoGame.Source.States.Components;

public class AnimatorComponentState : ComponentState
{
    public string CurrentState { get; set; }

    public AnimatorComponentState()
    {
    }

    public override void Serialize(NetDataWriter writer)
    {
        writer.Put(CurrentState);
    }

    public override void Deserialize(NetDataReader reader)
    {
        CurrentState = reader.GetString();
    }
}

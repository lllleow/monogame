using LiteNetLib.Utils;
using Microsoft.Xna.Framework;

namespace MonoGame.Source.States.Components;

public class AnimatorComponentState : ComponentState
{
    public string CurrentState { get; set; }
    public int CurrentTime { get; set; }
    public string AnimationBundleId { get; set; }

    public AnimatorComponentState()
    {
    }
}

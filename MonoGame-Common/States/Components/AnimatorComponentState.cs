namespace MonoGame_Common.States.Components;

public class AnimatorComponentState : ComponentState
{
    required public string CurrentState { get; set; }
    required public string AnimationBundleId { get; set; }
    public int CurrentTime { get; set; }
}
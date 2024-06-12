namespace MonoGame_Common.States.Components;

public class AnimatorComponentState : ComponentState
{
    public required string CurrentState { get; set; }
    public int CurrentTime { get; set; }
    public required string AnimationBundleId { get; set; }
}
namespace MonoGame.Source.Systems.Animation;

public class ChangeStateOnEndAnimationTransition : AnimationTransition
{
    public ChangeStateOnEndAnimationTransition(string from, string to) : base(from, to)
    {
        Condition = state => { return state.StateEnded; };
    }
}
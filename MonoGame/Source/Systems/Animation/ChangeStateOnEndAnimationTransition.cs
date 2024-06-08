using System;

namespace MonoGame;

public class ChangeStateOnEndAnimationTransition : AnimationTransition
{
    public ChangeStateOnEndAnimationTransition(string from, string to) : base(from, to)
    {
        Condition = (state) =>
        {
            return state.StateEnded;
        };
    }
}

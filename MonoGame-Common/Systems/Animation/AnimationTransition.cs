﻿namespace MonoGame_Common.Systems.Animation;

public class AnimationTransition
{
    public AnimationTransition(string from, string to)
    {
        From = from;
        To = to;
    }

    public AnimationTransition(string from, string to, Func<IAnimationState, bool> condition)
    {
        From = from;
        To = to;
        Condition = condition;
    }

    public string From { get; set; }
    public string To { get; set; }
    public Func<IAnimationState, bool> Condition { get; set; } = (state) => true;
}
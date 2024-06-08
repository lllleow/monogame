using System;
using MonoGame.Source.Systems.Entity.Interfaces;

namespace MonoGame;

public class AnimationTransition
{
    public string From { get; set; }
    public string To { get; set; }
    public Func<IAnimationState, bool> Condition { get; set; }

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
}

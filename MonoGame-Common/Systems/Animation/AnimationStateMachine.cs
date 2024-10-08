﻿namespace MonoGame_Common.Systems.Animation;

public class AnimationStateMachine
{
    public AnimationStateMachine(IAnimationBundle animationBundle)
    {
        AnimationBundle = animationBundle;
        foreach (var animation in animationBundle.Animations.Values)
        {
            AddState(new AnimationState(animation, animationBundle));
        }

        CurrentState = AnimationStates.Values.FirstOrDefault(state => state.Animation.IsDefault);
    }

    public IAnimationBundle AnimationBundle { get; set; }
    private Dictionary<string, IAnimationState> AnimationStates { get; } = [];
    public IAnimationState? CurrentState { get; set; }
    public Action<int, int> OnSpriteChanged { get; set; } = (x, y) => { };
    public Action<IAnimationState> OnStateEnded { get; set; } = (state) => { };

    public void AddState(IAnimationState state)
    {
        state.OnStateEnded = state => { OnStateEnded?.Invoke(state); };

        AnimationStates[state.Animation.Id] = state;
    }

    public void SetState(string animationId)
    {
        if (CurrentState?.Animation.Id != animationId)
        {
            var newState = AnimationStates[animationId];
            CurrentState = newState;
            CurrentState.Start();
        }
    }

    public void Update()
    {
        CurrentState?.Update();
        var (TextureX, TextureY) = CurrentState?.GetTextureCoordinates() ?? (0, 0);
        OnSpriteChanged?.Invoke(TextureX, TextureY);

        if (AnimationBundle?.AnimationTransitions != null && AnimationBundle.AnimationTransitions.Count > 0)
        {
            foreach (var transition in AnimationBundle.AnimationTransitions)
            {
                if (transition.From == CurrentState?.Animation.Id && transition.Condition(CurrentState))
                {
                    var newState = AnimationStates[transition.To];
                    CurrentState = newState;
                    CurrentState.Start();
                    break;
                }
            }
        }
    }
}
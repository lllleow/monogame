using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Source.Systems.Animation;

namespace MonoGame;

public class AnimationStateMachine
{
    public IAnimationBundle AnimationBundle { get; set; }
    private Dictionary<string, IAnimationState> AnimationStates { get; set; } = new();
    private IAnimationState CurrentState { get; set; }
    public Action<int, int> OnSpriteChanged { get; set; }
    public Action<IAnimationState> OnStateEnded { get; set; }

    public AnimationStateMachine(IAnimationBundle animationBundle)
    {
        foreach (Animation animation in animationBundle.Animations.Values)
        {
            AddState(new AnimationState(this, animation, animationBundle));
        }
    }

    public void AddState(IAnimationState state)
    {
        state.OnStateEnded = (state) =>
        {
            OnStateEnded?.Invoke(state);
        };

        AnimationStates[state.Animation.Id] = state;
    }

    public void SetState(string animationId)
    {
        IAnimationState newState = AnimationStates[animationId];
        CurrentState = newState;
        CurrentState.Start();
    }

    public void Update(GameTime gameTime)
    {
        CurrentState?.Update(gameTime);
        (int TextureX, int TextureY) = CurrentState?.GetTextureCoordinates() ?? (0, 0);
        OnSpriteChanged?.Invoke(TextureX, TextureY);

        if (AnimationBundle?.AnimationTransitions != null && AnimationBundle.AnimationTransitions.Count > 0)
        {
            foreach (AnimationTransition transition in AnimationBundle.AnimationTransitions)
            {
                if (transition.From == CurrentState.Animation.Id && transition.Condition(CurrentState))
                {
                    IAnimationState newState = AnimationStates[transition.To];
                    CurrentState = newState;
                    CurrentState.Start();
                    break;
                }
            }
        }
    }
}

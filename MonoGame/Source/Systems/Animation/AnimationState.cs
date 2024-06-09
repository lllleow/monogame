using System;
using Microsoft.Xna.Framework;
using MonoGame.Source.Systems.Animation;

namespace MonoGame;

public class AnimationState : IAnimationState
{
    public Animation Animation { get; set; }
    public IAnimationBundle AnimationBundle { get; set; }
    public Action<IAnimationState> OnStateEnded { get; set; } = (state) => { };
    public int CurrentTime { get; set; } = 0;
    public bool FinishedPlayingAnimation { get; set; } = false;
    public bool StateEnded { get; set; } = false;

    public AnimationState(Animation animation, IAnimationBundle animationBundle)
    {
        Animation = animation;
        AnimationBundle = animationBundle;
    }

    public double GetAnimationPercentage()
    {
        return CurrentTime / (double)Animation.Duration;
    }

    public (int TextureX, int TextureY) GetTextureCoordinates()
    {
        int TextureX = AnimationBundle.GetSpritesheetColumnForAnimationPercentage(Animation.Id, GetAnimationPercentage());
        int TextureY = AnimationBundle.GetSpritesheetRowForAnimation(Animation.Id);

        return (TextureX, TextureY);
    }

    public Rectangle GetTextureRectangle()
    {
        (int TextureX, int TextureY) = GetTextureCoordinates();
        return new Rectangle(TextureX * AnimationBundle.SizeX, TextureY * AnimationBundle.SizeY, AnimationBundle.SizeX, AnimationBundle.SizeY);
    }

    public void Start()
    {
        CurrentTime = 0;
        FinishedPlayingAnimation = false;
        StateEnded = false;
    }

    public void Update(GameTime gameTime)
    {
        CurrentTime++;
        if (CurrentTime > Animation.Duration)
        {
            FinishedPlayingAnimation = true;
            if (Animation.Repeats)
            {
                CurrentTime = Animation.Duration;
            }
            else
            {
                CurrentTime = 0;
            }
        }

        if (FinishedPlayingAnimation && !StateEnded)
        {
            StateEnded = true;
            OnStateEnded(this);
        }
    }
}

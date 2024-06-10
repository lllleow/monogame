using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Source.Systems.Animation;

public class AnimationState : IAnimationState
{
    public AnimationState(Animation animation, IAnimationBundle animationBundle)
    {
        Animation = animation;
        AnimationBundle = animationBundle;
    }

    public Animation Animation { get; set; }
    public IAnimationBundle AnimationBundle { get; set; }
    public Action<IAnimationState> OnStateEnded { get; set; } = state => { };
    public int CurrentTime { get; set; }
    public bool FinishedPlayingAnimation { get; set; }
    public bool StateEnded { get; set; }

    public double GetAnimationPercentage()
    {
        return CurrentTime / (double)Animation.Duration;
    }

    public (int TextureX, int TextureY) GetTextureCoordinates()
    {
        var TextureX =
            AnimationBundle.GetSpritesheetColumnForAnimationPercentage(Animation.Id, GetAnimationPercentage());
        var TextureY = AnimationBundle.GetSpritesheetRowForAnimation(Animation.Id);

        return (TextureX, TextureY);
    }

    public Rectangle GetTextureRectangle()
    {
        var (TextureX, TextureY) = GetTextureCoordinates();
        return new Rectangle(TextureX * AnimationBundle.SizeX, TextureY * AnimationBundle.SizeY, AnimationBundle.SizeX,
            AnimationBundle.SizeY);
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
            CurrentTime = Animation.Repeats ? Animation.Duration : 0;
        }

        if (FinishedPlayingAnimation && !StateEnded)
        {
            StateEnded = true;
            OnStateEnded(this);
        }
    }
}
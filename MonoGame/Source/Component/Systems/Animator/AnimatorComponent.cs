using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class AnimatorComponent : IEntityComponent
{
    int CurrentTime;
    public IGameEntity Entity { get; set; }
    public IAnimationBundle AnimationBundle;
    public string CurrentAnimation;

    public AnimatorComponent(IGameEntity entity, IAnimationBundle animation)
    {
        Entity = entity;
        AnimationBundle = animation;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Rectangle spriteRectangle = AnimationBundle.GetSpriteRectangle(CurrentAnimation, (double)CurrentTime / (double)AnimationBundle.Animations[CurrentAnimation].Duration);
        Console.Write(spriteRectangle);
        spriteBatch.Draw(SpritesheetLoader.GetSpritesheet(AnimationBundle.SpriteSheet), Entity.Position, spriteRectangle, Color.White);
    }

    public void Initialize()
    {
        CurrentAnimation ??= AnimationBundle.Animations.Keys.ToList().First();
    }

    public void PlayAnimation(string animationId)
    {
        if (animationId != CurrentAnimation)
        {
            CurrentTime = 0;
            CurrentAnimation = animationId;
        }
    }

    public void Update(GameTime gameTime)
    {
        CurrentTime++;
        if (CurrentTime > AnimationBundle.Animations[CurrentAnimation].Duration)
        {
            CurrentTime = 0;
        }
    }
}

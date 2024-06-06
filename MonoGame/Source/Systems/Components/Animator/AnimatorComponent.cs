using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Source.Systems.Animation;
using MonoGame.Source.Systems.Components.SpriteRenderer;
using MonoGame.Source.Systems.Entity.Interfaces;

namespace MonoGame.Source.Systems.Components.Animator;

public class AnimatorComponent : EntityComponent
{
    private int currentTime;
    private int currentTextureX = 0;
    private int currentTextureY = 0;

    public IAnimationBundle AnimationBundle;

    public string CurrentAnimation = "idle";

    public AnimatorComponent(IGameEntity entity, IAnimationBundle animation)
    {
        Entity = entity;
        AnimationBundle = animation;
    }

    public Rectangle GetSpriteRectangle()
    {
        return AnimationBundle.GetSpriteRectangle(CurrentAnimation, currentTime / (double)AnimationBundle.Animations[CurrentAnimation].Duration);
    }

    public override void Initialize()
    {
        if (!Entity.ContainsComponent<SpriteRendererComponent>())
        {
            throw new Exception("AnimatorComponent requires a SpriteRendererComponent to be present on the entity.");
        }

        CurrentAnimation ??= AnimationBundle.Animations.Keys.ToList().FirstOrDefault();
    }

    public void PlayAnimation(string animationId)
    {
        if (animationId != CurrentAnimation)
        {
            currentTime = 0;
            CurrentAnimation = animationId;
        }
    }

    public override void Update(GameTime gameTime)
    {
        currentTime++;
        if (currentTime > AnimationBundle.Animations[CurrentAnimation].Duration)
        {
            currentTime = 0;
        }

        int NewTextureX = AnimationBundle.GetSpritesheetColumnForAnimationPercentage(CurrentAnimation, currentTime / (double)AnimationBundle.Animations[CurrentAnimation].Duration);
        int NewTextureY = AnimationBundle.GetSpritesheetRowForAnimation(CurrentAnimation);

        if (NewTextureX != currentTextureX || NewTextureY != currentTextureY)
        {
            currentTextureX = NewTextureX;
            currentTextureY = NewTextureY;
            Entity.GetFirstComponent<SpriteRendererComponent>()?.UpdateTexture(AnimationBundle.SpriteSheet, new Rectangle(currentTextureX * AnimationBundle.SizeX, currentTextureY * AnimationBundle.SizeY, AnimationBundle.SizeX, AnimationBundle.SizeY));
        }
    }
}

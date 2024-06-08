using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Source.Systems.Animation;
using MonoGame.Source.Systems.Components.SpriteRenderer;
using MonoGame.Source.Systems.Entity.Interfaces;

namespace MonoGame.Source.Systems.Components.Animator;

public class AnimatorComponent : EntityComponent
{
    public int CurrentTime { get; set; }
    public int CurrentTextureX { get; set; } = 0;
    public int CurrentTextureY { get; set; } = 0;
    private AnimatorComponentNetworkController networkController = new();
    public IAnimationBundle AnimationBundle { get; set; }

    public string CurrentAnimation { get; set; } = "idle";

    public AnimatorComponent(IGameEntity entity, IAnimationBundle animation)
    {
        Entity = entity;
        AnimationBundle = animation;
        networkController.InitializeListeners(this);
    }

    public Rectangle GetSpriteRectangle()
    {
        return AnimationBundle.GetSpriteRectangle(CurrentAnimation, CurrentTime / (double)AnimationBundle.Animations[CurrentAnimation].Duration);
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
            CurrentTime = 0;
            CurrentAnimation = animationId;
        }
    }

    public override void Update(GameTime gameTime)
    {
        CurrentTime++;
        if (CurrentTime > AnimationBundle.Animations[CurrentAnimation].Duration)
        {
            CurrentTime = 0;
        }

        int NewTextureX = AnimationBundle.GetSpritesheetColumnForAnimationPercentage(CurrentAnimation, CurrentTime / (double)AnimationBundle.Animations[CurrentAnimation].Duration);
        int NewTextureY = AnimationBundle.GetSpritesheetRowForAnimation(CurrentAnimation);

        if (NewTextureX != CurrentTextureX || NewTextureY != CurrentTextureY)
        {
            CurrentTextureX = NewTextureX;
            CurrentTextureY = NewTextureY;
            Entity.GetFirstComponent<SpriteRendererComponent>()?.UpdateTexture(AnimationBundle.SpriteSheet, new Rectangle(CurrentTextureX * AnimationBundle.SizeX, CurrentTextureY * AnimationBundle.SizeY, AnimationBundle.SizeX, AnimationBundle.SizeY));
            networkController.SendStateUpdate(this);
        }
    }
}

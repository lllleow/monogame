using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Systems.Components.Interfaces;
using MonoGame.Source.Systems.Entity.Interfaces;
using MonoGame.Source.Util.Loaders;

namespace MonoGame.Source.Systems.Components.Animator;

public class AnimatorComponent : IEntityComponent
{
    /// <summary>
    /// The current time of the animation.
    /// </summary>
    int CurrentTime;

    /// <summary>
    /// The current X texture index.
    /// </summary>
    int CurrentTextureX = 0;

    /// <summary>
    /// The current Y texture index.
    /// </summary>
    int CurrentTextureY = 0;

    /// <summary>
    /// The entity associated with this animator component.
    /// </summary>
    public IGameEntity Entity { get; set; }

    /// <summary>
    /// The animation bundle containing all the animations for this component.
    /// </summary>
    public IAnimationBundle AnimationBundle;

    /// <summary>
    /// The ID of the current animation being played.
    /// </summary>
    public string CurrentAnimation;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnimatorComponent"/> class.
    /// </summary>
    /// <param name="entity">The entity associated with this animator component.</param>
    /// <param name="animation">The animation bundle containing all the animations for this component.</param>
    public AnimatorComponent(IGameEntity entity, IAnimationBundle animation)
    {
        Entity = entity;
        AnimationBundle = animation;
    }

    /// <summary>
    /// Draws the current frame of the animation.
    /// </summary>
    /// <param name="spriteBatch">The sprite batch used for drawing.</param>
    public void Draw(SpriteBatch spriteBatch)
    {
    }

    /// <summary>
    /// Gets the rectangle representing the current frame of the animation.
    /// </summary>
    /// <returns>The rectangle representing the current frame of the animation.</returns>
    public Rectangle GetSpriteRectangle()
    {
        return AnimationBundle.GetSpriteRectangle(CurrentAnimation, CurrentTime / (double)AnimationBundle.Animations[CurrentAnimation].Duration);
    }

    /// <summary>
    /// Initializes the AnimatorComponent.
    /// </summary>
    public void Initialize()
    {
        if (!Entity.ContainsComponent<SpriteRendererComponent>())
        {
            throw new Exception("AnimatorComponent requires a SpriteRendererComponent to be present on the entity.");
        }

        CurrentAnimation ??= AnimationBundle.Animations.Keys.ToList().First();
    }

    /// <summary>
    /// Plays the specified animation.
    /// </summary>
    /// <param name="animationId">The ID of the animation to play.</param>
    public void PlayAnimation(string animationId)
    {
        if (animationId != CurrentAnimation)
        {
            CurrentTime = 0;
            CurrentAnimation = animationId;
        }
    }

    /// <summary>
    /// Updates the animator component.
    /// </summary>
    /// <param name="gameTime">The game time.</param>
    public void Update(GameTime gameTime)
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
        }
    }
}

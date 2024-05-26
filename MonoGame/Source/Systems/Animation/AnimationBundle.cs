using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoGame;

/// <summary>
/// Represents a bundle of animations.
/// </summary>
public class AnimationBundle : IAnimationBundle
{
    public string Id { get; set; }
    public string SpriteSheet { get; set; }
    public int SizeX { get; set; } = 1;
    public int SizeY { get; set; } = 1;
    public Dictionary<string, Animation> Animations { get; set; } = new Dictionary<string, Animation>();

    /// <summary>
    /// Gets the sprite rectangle for the specified animation and percentage.
    /// </summary>
    /// <param name="animationId">The ID of the animation.</param>
    /// <param name="percentage">The percentage of the animation progress.</param>
    /// <returns>The sprite rectangle.</returns>
    public Rectangle GetSpriteRectangle(string animationId, double percentage)
    {
        Rectangle rect = new Rectangle(GetSpritesheetColumnForAnimationPercentage(animationId, percentage) * Tile.PixelSizeX, GetSpritesheetRowForAnimation(animationId) * Tile.PixelSizeY, SizeX * Tile.PixelSizeX, SizeY * Tile.PixelSizeY);
        return rect;
    }

    /// <summary>
    /// Gets the spritesheet column for the specified animation and percentage.
    /// </summary>
    /// <param name="animationId">The ID of the animation.</param>
    /// <param name="percentage">The percentage of the animation progress.</param>
    /// <returns>The spritesheet column.</returns>
    public int GetSpritesheetColumnForAnimationPercentage(string animationId, double percentage)
    {
        int column = (int)(Animations[animationId].SpriteCount * percentage);
        if (column > Animations[animationId].SpriteCount)
        {
            return column - 1;
        }
        return column;
    }

    /// <summary>
    /// Gets the spritesheet row for the specified animation.
    /// </summary>
    /// <param name="animationName">The name of the animation.</param>
    /// <returns>The spritesheet row.</returns>
    public int GetSpritesheetRowForAnimation(string animationName)
    {
        return Animations.Keys.ToList().IndexOf(animationName);
    }

    /// <summary>
    /// Creates a new animation and adds it to the bundle.
    /// </summary>
    /// <param name="animation">The animation to create.</param>
    /// <exception cref="Exception">Thrown when an animation with the same ID already exists in the bundle.</exception>
    public void CreateAnimation(Animation animation)
    {
        if (Animations.ContainsKey(animation.Id))
        {
            throw new Exception("Animation already registered " + animation.Id);
        }
        else
        {
            Animations[animation.Id] = animation;
        }
    }
}

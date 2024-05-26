using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGame;

/// <summary>
/// Represents a bundle of animations for a sprite sheet.
/// </summary>
public interface IAnimationBundle
{
    /// <summary>
    /// Gets or sets the sprite sheet associated with the animation bundle.
    /// </summary>
    string SpriteSheet { get; set; }

    /// <summary>
    /// Gets or sets the width of each frame in the sprite sheet.
    /// </summary>
    int SizeX { get; set; }

    /// <summary>
    /// Gets or sets the height of each frame in the sprite sheet.
    /// </summary>
    int SizeY { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the animation bundle.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// Gets or sets the dictionary of animations contained in the bundle.
    /// </summary>
    Dictionary<string, Animation> Animations { get; set; }

    /// <summary>
    /// Gets the column index in the sprite sheet for a given animation and percentage of completion.
    /// </summary>
    /// <param name="animationName">The name of the animation.</param>
    /// <param name="percentage">The percentage of completion of the animation.</param>
    /// <returns>The column index in the sprite sheet.</returns>
    int GetSpritesheetColumnForAnimationPercentage(string animationName, double percentage);

    /// <summary>
    /// Gets the row index in the sprite sheet for a given animation.
    /// </summary>
    /// <param name="animationName">The name of the animation.</param>
    /// <returns>The row index in the sprite sheet.</returns>
    int GetSpritesheetRowForAnimation(string animationName);

    /// <summary>
    /// Creates a new animation and adds it to the animation bundle.
    /// </summary>
    /// <param name="animation">The animation to create.</param>
    void CreateAnimation(Animation animation);

    /// <summary>
    /// Gets the rectangle in the sprite sheet for a given animation and percentage of completion.
    /// </summary>
    /// <param name="animationName">The name of the animation.</param>
    /// <param name="percentage">The percentage of completion of the animation.</param>
    /// <returns>The rectangle in the sprite sheet.</returns>
    Rectangle GetSpriteRectangle(string animationName, double percentage);
}

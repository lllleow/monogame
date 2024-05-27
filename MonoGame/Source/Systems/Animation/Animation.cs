namespace MonoGame;

/// <summary>
/// Represents an animation.
/// </summary>
public class Animation
{
    /// <summary>
    /// Gets or sets the ID of the animation.
    /// </summary>
    public string Id { get; set; }
    /// <summary>
    /// Gets or sets the row index of the sprite in the spritesheet.
    /// </summary>
    public int SpritesheetRow { get; set; }

    /// <summary>
    /// Gets or sets the duration of the animation in milliseconds.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets the number of sprites in the animation.
    /// </summary>
    public int SpriteCount { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Animation"/> class.
    /// </summary>
    /// <param name="id">The ID of the animation.</param>
    /// <param name="duration">The duration of the animation in milliseconds.</param>
    /// <param name="spriteCount">The number of sprites in the animation.</param>
    public Animation(string id, int row, int duration, int spriteCount)
    {
        Id = id;
        Duration = duration;
        SpriteCount = spriteCount;
        SpritesheetRow = row;
    }
}

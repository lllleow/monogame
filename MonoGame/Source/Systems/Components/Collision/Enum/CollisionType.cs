namespace MonoGame;

/// <summary>
/// Represents the type of collision used in the game.
/// </summary>
public enum CollisionMode
{
    /// <summary>
    /// Collision detection is based on a bounding box.
    /// </summary>
    BoundingBox,

    /// <summary>
    /// Collision detection is based on pixel-perfect accuracy.
    /// </summary>
    PixelPerfect,

    /// <summary>
    /// Collision detection is based on collision masks with pixel-perfect accuracy.
    /// </summary>
    CollisionMask
}

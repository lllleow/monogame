namespace MonoGame;

/// <summary>
/// Represents the type of collision used in the game.
/// </summary>
public enum CollisionType
{
    /// <summary>
    /// Collision detection is based on full tiles.
    /// </summary>
    FullTile,

    /// <summary>
    /// Collision detection is based on pixel-perfect accuracy.
    /// </summary>
    PixelPerfect,

    /// <summary>
    /// Collision detection is based on collision masks.
    /// </summary>
    CollisionMask
}

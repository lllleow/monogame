namespace MonoGame;

/// <summary>
/// Represents the possible directions.
/// </summary>
public enum Direction
{
    Up,
    Down,
    Right,
    Left,
    RightUp,
    RightDown,
    LeftUp,
    LeftDown
}

public class DirectionHelper
{
    public static Direction GetOpositeDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            case Direction.LeftUp:
                return Direction.RightDown;
            case Direction.RightUp:
                return Direction.LeftDown;
            case Direction.LeftDown:
                return Direction.RightUp;
            case Direction.RightDown:
                return Direction.LeftUp;
            default:
                return Direction.Up;
        }
    }

    /// <summary>
    /// Gets the direction based on the specified X and Y coordinates.
    /// </summary>
    /// <param name="x">The X coordinate.</param>
    /// <param name="y">The Y coordinate.</param>
    /// <returns>The direction.</returns>
    public static Direction GetDirection(int x, int y)
    {
        if (x == 0 && y > 0) return Direction.Up;
        if (x == 0 && y < 0) return Direction.Down;
        if (x > 0 && y == 0) return Direction.Right;
        if (x > 0 && y > 0) return Direction.RightUp;
        if (x > 0 && y < 0) return Direction.RightDown;
        if (x < 0 && y == 0) return Direction.Left;
        if (x < 0 && y > 0) return Direction.LeftUp;
        if (x < 0 && y < 0) return Direction.LeftDown;
        return Direction.Up;
    }
}

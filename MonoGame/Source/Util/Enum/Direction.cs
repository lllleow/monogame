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
}

namespace MonoGame_Common.Util.Enum;

public enum Direction
{
    Up,
    Down,
    Right,
    Left,
    RightUp,
    RightDown,
    LeftUp,
    LeftDown,
    None
}

public class DirectionHelper
{
    public static Direction GetOpositeDirection(Direction direction)
    {
        return direction switch
        {
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            Direction.Right => Direction.Left,
            Direction.LeftUp => Direction.RightDown,
            Direction.RightUp => Direction.LeftDown,
            Direction.LeftDown => Direction.RightUp,
            Direction.RightDown => Direction.LeftUp,
            _ => Direction.Up,
        };
    }

    public static Direction GetDirection(int x, int y)
    {
        return x == 0 && y > 0
            ? Direction.Up
            : x == 0 && y < 0
            ? Direction.Down
            : x > 0 && y == 0
            ? Direction.Right
            : x > 0 && y > 0
            ? Direction.RightUp
            : x > 0 && y < 0
            ? Direction.RightDown
            : x < 0 && y == 0 ? Direction.Left : x < 0 && y > 0 ? Direction.LeftUp : x < 0 && y < 0 ? Direction.LeftDown : Direction.Up;
    }
}

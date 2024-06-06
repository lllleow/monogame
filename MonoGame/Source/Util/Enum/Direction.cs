namespace MonoGame.Source.Util.Enum;

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
        if (x == 0 && y > 0)
        {
            return Direction.Up;
        }

        if (x == 0 && y < 0)
        {
            return Direction.Down;
        }

        if (x > 0 && y == 0)
        {
            return Direction.Right;
        }

        if (x > 0 && y > 0)
        {
            return Direction.RightUp;
        }

        return x > 0 && y < 0
            ? Direction.RightDown
            : x < 0 && y == 0 ? Direction.Left : x < 0 && y > 0 ? Direction.LeftUp : x < 0 && y < 0 ? Direction.LeftDown : Direction.Up;
    }
}

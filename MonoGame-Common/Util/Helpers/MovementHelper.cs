using System.Numerics;
using MonoGame_Common.Util.Enum;

namespace MonoGame_Common.Util.Helpers;

public class MovementHelper
{
    public static Vector2 GetDisplacement(Direction direction, Vector2 speed)
    {
        return direction switch
        {
            Direction.Up => new Vector2(0, -speed.Y),
            Direction.Down => new Vector2(0, speed.Y),
            Direction.Left => new Vector2(-speed.X, 0),
            Direction.Right => new Vector2(speed.X, 0),
            Direction.LeftUp => new Vector2(-speed.X, -speed.Y),
            Direction.RightUp => new Vector2(speed.X, -speed.Y),
            Direction.LeftDown => new Vector2(-speed.X, speed.Y),
            Direction.RightDown => new Vector2(speed.X, speed.Y),
            _ => Vector2.Zero,
        };
    }
}

using System.Drawing;

namespace MonoGame_Common.Util;

public class CollisionMaskHandler
{
    public static Dictionary<int, bool[,]> CollisionMasks { get; set; } = [];

    public static bool CheckMaskCollision(bool[,] mask1, Rectangle rect1, bool[,] mask2, Rectangle rect2)
    {
        var overlapX = Math.Max(rect1.X, rect2.X);
        var overlapY = Math.Max(rect1.Y, rect2.Y);
        var overlapWidth = Math.Min(rect1.X + rect1.Width, rect2.X + rect2.Width) - overlapX;
        var overlapHeight = Math.Min(rect1.Y + rect1.Height, rect2.Y + rect2.Height) - overlapY;

        if (overlapWidth <= 0 || overlapHeight <= 0)
        {
            return false;
        }

        for (var y = 0; y < overlapHeight; y++)
        {
            for (var x = 0; x < overlapWidth; x++)
            {
                var mask1X = overlapX - rect1.X + x;
                var mask1Y = overlapY - rect1.Y + y;
                var mask2X = overlapX - rect2.X + x;
                var mask2Y = overlapY - rect2.Y + y;

                if (mask1X < 0 || mask1Y < 0 || mask1X >= mask1.GetLength(0) || mask1Y >= mask1.GetLength(1))
                {
                    continue;
                }

                if (mask2X < 0 || mask2Y < 0 || mask2X >= mask2.GetLength(0) || mask2Y >= mask2.GetLength(1))
                {
                    continue;
                }

                if (mask1[mask1X, mask1Y] && mask2[mask2X, mask2Y])
                {
                    return true;
                }
            }
        }

        return false;
    }
}
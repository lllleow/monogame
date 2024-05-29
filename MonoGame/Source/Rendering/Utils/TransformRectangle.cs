using System;
using Microsoft.Xna.Framework;

namespace MonoGame;

public static class TransformRectangle
{
    public static Rectangle Transform(Rectangle rectangle, Matrix transform)
    {
        Vector2 topLeft = new Vector2(rectangle.Left, rectangle.Top);
        Vector2 topRight = new Vector2(rectangle.Right, rectangle.Top);
        Vector2 bottomLeft = new Vector2(rectangle.Left, rectangle.Bottom);
        Vector2 bottomRight = new Vector2(rectangle.Right, rectangle.Bottom);

        Vector2 transformedTopLeft = Vector2.Transform(topLeft, transform);
        Vector2 transformedTopRight = Vector2.Transform(topRight, transform);
        Vector2 transformedBottomLeft = Vector2.Transform(bottomLeft, transform);
        Vector2 transformedBottomRight = Vector2.Transform(bottomRight, transform);

        Vector2 min = Vector2.Min(Vector2.Min(transformedTopLeft, transformedTopRight),
                                  Vector2.Min(transformedBottomLeft, transformedBottomRight));
        Vector2 max = Vector2.Max(Vector2.Max(transformedTopLeft, transformedTopRight),
                                  Vector2.Max(transformedBottomLeft, transformedBottomRight));

        return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
    }
}

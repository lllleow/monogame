using Microsoft.Xna.Framework;

namespace MonoGame.Source.Rendering.Utils;

public static class TransformRectangle
{
    public static Rectangle Transform(Rectangle rectangle, Matrix transform)
    {
        var topLeft = new Vector2(rectangle.Left, rectangle.Top);
        var topRight = new Vector2(rectangle.Right, rectangle.Top);
        var bottomLeft = new Vector2(rectangle.Left, rectangle.Bottom);
        var bottomRight = new Vector2(rectangle.Right, rectangle.Bottom);

        var transformedTopLeft = Vector2.Transform(topLeft, transform);
        var transformedTopRight = Vector2.Transform(topRight, transform);
        var transformedBottomLeft = Vector2.Transform(bottomLeft, transform);
        var transformedBottomRight = Vector2.Transform(bottomRight, transform);

        var min = Vector2.Min(
            Vector2.Min(transformedTopLeft, transformedTopRight),
            Vector2.Min(transformedBottomLeft, transformedBottomRight));
        var max = Vector2.Max(
            Vector2.Max(transformedTopLeft, transformedTopRight),
            Vector2.Max(transformedBottomLeft, transformedBottomRight));

        return new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
    }
}
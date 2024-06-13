using System;
using Microsoft.Xna.Framework;
using MonoGame_Common;
using MonoGame_Common.States;
namespace MonoGame.Source.Utils.Helpers;

public static class RectangleHelper
{
    public static Rectangle GetMinimumBoundingRectangle(Rectangle[] rectangles)
    {
        var minX = int.MaxValue;
        var minY = int.MaxValue;
        var maxX = int.MinValue;
        var maxY = int.MinValue;

        foreach (var rectangle in rectangles)
        {
            minX = Math.Min(minX, rectangle.Left);
            minY = Math.Min(minY, rectangle.Top);
            maxX = Math.Max(maxX, rectangle.Right);
            maxY = Math.Max(maxY, rectangle.Bottom);
        }

        var width = maxX - minX;
        var height = maxY - minY;

        return new Rectangle(minX, minY, width, height);
    }

    public static Rectangle ConvertToXNARectangle(System.Drawing.Rectangle rectangle)
    {
        return new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
    }

    public static System.Drawing.Rectangle ConvertToDrawingRectangle(Rectangle rectangle)
    {
        return new System.Drawing.Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
    }

    public static Rectangle GetTextureRectangleFromCoordinates(int x, int y)
    {
        return new Rectangle(x * SharedGlobals.PixelSizeX, y * SharedGlobals.PixelSizeY, SharedGlobals.PixelSizeX, SharedGlobals.PixelSizeY);
    }
}
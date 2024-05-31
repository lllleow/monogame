
using System;
using Microsoft.Xna.Framework;

namespace MonoGame
{
    public class RectangleHelper
    {
        public Rectangle GetMinimumBoundingRectangle(Rectangle[] rectangles)
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            foreach (var rectangle in rectangles)
            {
                minX = Math.Min(minX, rectangle.Left);
                minY = Math.Min(minY, rectangle.Top);
                maxX = Math.Max(maxX, rectangle.Right);
                maxY = Math.Max(maxY, rectangle.Bottom);
            }

            int width = maxX - minX;
            int height = maxY - minY;

            return new Rectangle(minX, minY, width, height);
        }

        public Rectangle GetTextureRectangleFromCoordinates(int x, int y)
        {
            return new Rectangle(x * Tile.PixelSizeX, y * Tile.PixelSizeX, Tile.PixelSizeX, Tile.PixelSizeY);
        }
    }
}
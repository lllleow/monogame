using System;
using Microsoft.Xna.Framework;
using MonoGame.Source.Systems.Tiles;

namespace MonoGame.Source.Util.Helpers
{
    public class RectangleHelper
    {
        public Rectangle GetMinimumBoundingRectangle(Rectangle[] rectangles)
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

        public Rectangle GetTextureRectangleFromCoordinates(int x, int y)
        {
            return new Rectangle(x * Tile.PixelSizeX, y * Tile.PixelSizeX, Tile.PixelSizeX, Tile.PixelSizeY);
        }
    }
}
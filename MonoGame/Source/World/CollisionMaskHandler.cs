using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class CollisionMaskHandler
{
    public static Dictionary<int, bool[,]> CollisionMasks = new Dictionary<int, bool[,]>();

    public static bool[,] GetMaskForTexture(string spritesheet, Rectangle region)
    {
        Tuple<string, Rectangle> tuple = new Tuple<string, Rectangle>(spritesheet, region);
        int key = tuple.GetHashCode();

        if (CollisionMasks.ContainsKey(key))
        {
            return CollisionMasks[key];
        }
        else
        {
            bool[,] mask = CreateCollisionMask(spritesheet, region);
            CollisionMasks.Add(key, mask);
            return mask;
        }
    }

    public static bool[,] CreateCollisionMask(string spritesheet, Rectangle region)
    {
        Texture2D texture = SpritesheetLoader.GetSpritesheet(spritesheet);

        int startX = Math.Max(region.X, 0);
        int startY = Math.Max(region.Y, 0);
        int endX = Math.Min(startX + region.Width, texture.Width);
        int endY = Math.Min(startY + region.Height, texture.Height);

        int width = endX - startX;
        int height = endY - startY;

        Color[] textureData = new Color[width * height];
        try
        {
            texture.GetData(0, region, textureData, 0, textureData.Length);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error loading texture data for " + spritesheet + " " + region);
            return new bool[0, 0];
        }

        bool[,] mask = new bool[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = x + y * width;
                mask[x, y] = textureData[index].A != 0;
            }
        }
        return mask;
    }

    public static bool CheckMaskCollision(bool[,] mask1, Rectangle rect1, bool[,] mask2, Rectangle rect2)
    {
        // Calculate the overlapping region
        int overlapX = Math.Max(rect1.X, rect2.X);
        int overlapY = Math.Max(rect1.Y, rect2.Y);
        int overlapWidth = Math.Min(rect1.X + rect1.Width, rect2.X + rect2.Width) - overlapX;
        int overlapHeight = Math.Min(rect1.Y + rect1.Height, rect2.Y + rect2.Height) - overlapY;

        // Early exit if there is no overlap
        if (overlapWidth <= 0 || overlapHeight <= 0)
            return false;

        // Check every point in the overlapping area
        for (int y = 0; y < overlapHeight; y++)
        {
            for (int x = 0; x < overlapWidth; x++)
            {
                // Coordinates in masks
                int mask1X = overlapX - rect1.X + x;
                int mask1Y = overlapY - rect1.Y + y;
                int mask2X = overlapX - rect2.X + x;
                int mask2Y = overlapY - rect2.Y + y;

                // Bounds check for mask1
                if (mask1X < 0 || mask1Y < 0 || mask1X >= mask1.GetLength(0) || mask1Y >= mask1.GetLength(1))
                    continue;

                // Bounds check for mask2
                if (mask2X < 0 || mask2Y < 0 || mask2X >= mask2.GetLength(0) || mask2Y >= mask2.GetLength(1))
                    continue;

                // Check if both masks are solid at this point
                if (mask1[mask1X, mask1Y] && mask2[mask2X, mask2Y])
                    return true;
            }
        }

        return false;
    }
}

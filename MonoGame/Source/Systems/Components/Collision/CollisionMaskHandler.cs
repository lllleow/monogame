using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Source.Util.Loaders;

namespace MonoGame.Source.Systems.Components.Collision;

/// <summary>
/// Provides methods for handling collision masks used in collision detection.
/// </summary>
public class CollisionMaskHandler
{
    /// <summary>
    /// A dictionary that stores collision masks for different textures.
    /// The key is a hash code generated from the spritesheet and region.
    /// The value is a 2D boolean array representing the collision mask.
    /// </summary>
    public static Dictionary<int, bool[,]> CollisionMasks = new Dictionary<int, bool[,]>();

    /// <summary>
    /// Retrieves the collision mask for a given texture and region.
    /// If the collision mask is already stored in the dictionary, it is returned.
    /// Otherwise, a new collision mask is created, stored in the dictionary, and returned.
    /// </summary>
    /// <param name="spritesheet">The name of the spritesheet.</param>
    /// <param name="region">The region of the texture to create the collision mask for.</param>
    /// <returns>The collision mask as a 2D boolean array.</returns>
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

    /// <summary>
    /// Creates a collision mask for a given texture and region.
    /// The collision mask is a 2D boolean array where each element represents
    /// whether a pixel in the texture is considered solid or not.
    /// </summary>
    /// <param name="spritesheet">The name of the spritesheet.</param>
    /// <param name="region">The region of the texture to create the collision mask for.</param>
    /// <returns>The collision mask as a 2D boolean array.</returns>
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
            Console.WriteLine("Error loading texture data for " + spritesheet + " " + region + " " + e);
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

    /// <summary>
    /// Checks for collision between two collision masks within their respective rectangles.
    /// </summary>
    /// <param name="mask1">The collision mask of the first object.</param>
    /// <param name="rect1">The rectangle that defines the position and size of the first object.</param>
    /// <param name="mask2">The collision mask of the second object.</param>
    /// <param name="rect2">The rectangle that defines the position and size of the second object.</param>
    /// <returns>True if there is a collision, false otherwise.</returns>
    public static bool CheckMaskCollision(bool[,] mask1, Rectangle rect1, bool[,] mask2, Rectangle rect2)
    {
        int overlapX = Math.Max(rect1.X, rect2.X);
        int overlapY = Math.Max(rect1.Y, rect2.Y);
        int overlapWidth = Math.Min(rect1.X + rect1.Width, rect2.X + rect2.Width) - overlapX;
        int overlapHeight = Math.Min(rect1.Y + rect1.Height, rect2.Y + rect2.Height) - overlapY;

        if (overlapWidth <= 0 || overlapHeight <= 0)
            return false;

        for (int y = 0; y < overlapHeight; y++)
        {
            for (int x = 0; x < overlapWidth; x++)
            {
                int mask1X = overlapX - rect1.X + x;
                int mask1Y = overlapY - rect1.Y + y;
                int mask2X = overlapX - rect2.X + x;
                int mask2Y = overlapY - rect2.Y + y;

                if (mask1X < 0 || mask1Y < 0 || mask1X >= mask1.GetLength(0) || mask1Y >= mask1.GetLength(1))
                    continue;

                if (mask2X < 0 || mask2Y < 0 || mask2X >= mask2.GetLength(0) || mask2Y >= mask2.GetLength(1))
                    continue;

                if (mask1[mask1X, mask1Y] && mask2[mask2X, mask2Y])
                    return true;
            }
        }

        return false;
    }
}

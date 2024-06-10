using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Source.Utils.Loaders;

namespace MonoGame.Source.Systems.Components.Collision;

public class CollisionMaskHandler
{
    public static Dictionary<int, bool[,]> CollisionMasks { get; set; } = [];

    public static bool[,] GetMaskForTexture(string spritesheet, Rectangle region)
    {
        var tuple = new Tuple<string, Rectangle>(spritesheet, region);
        var key = tuple.GetHashCode();

        if (CollisionMasks.ContainsKey(key))
        {
            return CollisionMasks[key];
        }

        var mask = CreateCollisionMask(spritesheet, region);
        CollisionMasks.Add(key, mask);
        return mask;
    }

    public static bool[,] CreateCollisionMask(string spritesheet, Rectangle region)
    {
        var texture = SpritesheetLoader.GetSpritesheet(spritesheet);

        var startX = Math.Max(region.X, 0);
        var startY = Math.Max(region.Y, 0);
        var endX = Math.Min(startX + region.Width, texture.Width);
        var endY = Math.Min(startY + region.Height, texture.Height);

        var width = endX - startX;
        var height = endY - startY;

        var textureData = new Color[width * height];
        try
        {
            texture.GetData(0, region, textureData, 0, textureData.Length);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error loading texture data for " + spritesheet + " " + region + " " + e);
            return new bool[0, 0];
        }

        var mask = new bool[width, height];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var index = x + (y * width);
                mask[x, y] = textureData[index].A != 0;
            }
        }

        return mask;
    }

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
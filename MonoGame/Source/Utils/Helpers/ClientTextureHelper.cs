using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoGame.Source.Utils.Loaders;

namespace MonoGame;

public static class ClientTextureHelper
{
    public static Dictionary<int, bool[,]> CollisionMasks { get; set; } = [];

    public static bool[,] GetMaskForTexture(string spritesheet, Rectangle region)
    {
        var tuple = new Tuple<string, Rectangle>(spritesheet, region);
        var key = tuple.GetHashCode();

        if (CollisionMasks.TryGetValue(key, out var val))
        {
            return val;
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
}

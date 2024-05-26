using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoGame;

public class AnimationBundle : IAnimationBundle
{
    public string Id { get; set; }
    public string SpriteSheet { get; set; }
    public int SizeX { get; set; } = 1;
    public int SizeY { get; set; } = 1;
    public Dictionary<string, Animation> Animations { get; set; } = new Dictionary<string, Animation>();

    public Rectangle GetSpriteRectangle(string animationId, double percentage)
    {
        Rectangle rect = new Rectangle(GetSpritesheetColumnForAnimationPercentage(animationId, percentage) * Tile.PixelSizeX, GetSpritesheetRowForAnimation(animationId) * Tile.PixelSizeY, SizeX * Tile.PixelSizeX, SizeY * Tile.PixelSizeY);
        return rect;
    }

    public int GetSpritesheetColumnForAnimationPercentage(string animationId, double percentage)
    {
        int column = (int)(Animations[animationId].SpriteCount * percentage);
        if (column > Animations[animationId].SpriteCount)
        {
            return column - 1;
        }
        return column;
    }

    public int GetSpritesheetRowForAnimation(string animationName)
    {
        return Animations.Keys.ToList().IndexOf(animationName);
    }

    public void CreateAnimation(Animation animation)
    {
        if (Animations.ContainsKey(animation.Id))
        {
            throw new Exception("Animation already registered " + animation.Id);
        }
        else
        {
            Animations[animation.Id] = animation;
        }
    }
}

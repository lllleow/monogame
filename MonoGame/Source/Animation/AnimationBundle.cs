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
        return new Rectangle(GetSpritesheetColumnForAnimationPercentage(animationId, percentage) * Tile.PixelSizeX * 2, GetSpritesheetRowForAnimation(animationId) * Tile.PixelSizeY * 2, SizeX * Tile.PixelSizeX * 2, SizeY * Tile.PixelSizeY * 2);
    }

    public int GetSpritesheetColumnForAnimationPercentage(string animationId, double percentage)
    {
        return (int)(Animations[animationId].SpriteCount * percentage);
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

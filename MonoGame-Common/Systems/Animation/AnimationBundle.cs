﻿using System.Drawing;

namespace MonoGame_Common.Systems.Animation;

public class AnimationBundle : IAnimationBundle
{
    public string? Id { get; set; }
    public string? SpriteSheet { get; set; }
    public string? CollisionMaskSpritesheet { get; set; }
    public int SizeX { get; set; } = 16;
    public int SizeY { get; set; } = 16;
    public Dictionary<string, Animation> Animations { get; set; } = [];
    public List<AnimationTransition> AnimationTransitions { get; set; } = [];

    public Rectangle GetSpriteRectangle(string animationId, double percentage)
    {
        var rect = new Rectangle(
            GetSpritesheetColumnForAnimationPercentage(animationId, percentage) * SharedGlobals.PixelSizeX,
            GetSpritesheetRowForAnimation(animationId) * SharedGlobals.PixelSizeY,
            SizeX * SharedGlobals.PixelSizeX,
            SizeY * SharedGlobals.PixelSizeY);
        return rect;
    }

    public int GetSpritesheetColumnForAnimationPercentage(string animationId, double percentage)
    {
        var column = (int)(Animations[animationId].SpriteCount * percentage);
        return column > Animations[animationId].SpriteCount ? column - 1 : column;
    }

    public int GetSpritesheetRowForAnimation(string animationName)
    {
        return Animations[animationName].SpritesheetRow;
    }

    public void CreateAnimation(Animation animation)
    {
        Animations[animation.Id] = Animations.ContainsKey(animation.Id)
            ? throw new Exception("Animation already registered " + animation.Id)
            : animation;
    }

    public void AddTransition(AnimationTransition animationTransition)
    {
        AnimationTransitions.Add(animationTransition);
    }
}
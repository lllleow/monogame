﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGame;

public interface IAnimationBundle
{
    string SpriteSheet { get; set; }
    int SizeX { get; set; }
    int SizeY { get; set; }
    string Id { get; set; }
    Dictionary<string, Animation> Animations { get; set; }
    int GetSpritesheetColumnForAnimationPercentage(string animationName, double percentage);
    int GetSpritesheetRowForAnimation(string animationName);
    void CreateAnimation(Animation animation);
    Rectangle GetSpriteRectangle(string animationName, double percentage);
}

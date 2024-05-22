using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoGame;

public class Animation : IAnimation
{
    public string SpriteSheet { get; set; }
    public string CurrentState { get; set; }
    public int SizeX { get; set; } = 1;
    public int SizeY { get; set; } = 1;
    public Dictionary<string, int> AnimationStates { get; set; } = new Dictionary<string, int>();
    public Dictionary<string, int> AnimationSpriteCount { get; set; } = new Dictionary<string, int>();
    public float Duration { get; set; } = 100;
    public string Id { get; set; }

    public void SetState(string state)
    {
        CurrentState = state;
    }

    public int GetColumnForPercentage(double percentage)
    {
        return (int)(AnimationSpriteCount[CurrentState] * percentage);
    }

    public int GetRowForState(string state)
    {
        return AnimationStates.Keys.ToList().IndexOf(state);
    }

    public Rectangle GetSpriteRectangle(double percentage)
    {
        return new Rectangle(GetColumnForPercentage(percentage) * Tile.PixelSizeX, GetRowForState(CurrentState) * Tile.PixelSizeY, SizeX * Tile.PixelSizeX, SizeY * Tile.PixelSizeY);
    }

    public void RegisterRowForState(string state, int row, int spriteCount)
    {
        if (AnimationStates.ContainsKey(state))
        {
            throw new Exception("Row already registered for state " + state);
        }
        else
        {
            AnimationStates[state] = row;
        }


        if (AnimationSpriteCount.ContainsKey(state))
        {
            throw new Exception("Duration already registered for state " + state);
        }
        else
        {
            AnimationSpriteCount[state] = spriteCount;
        }
    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGame;

public interface IAnimation
{
    string SpriteSheet { get; set; }
    float Duration { get; set; }
    string CurrentState { get; set; }
    int SizeX { get; set; }
    int SizeY { get; set; }
    Dictionary<string, int> AnimationStates { get; set; }
    int GetColumnForPercentage(double percentage);
    int GetRowForState(string state);
    void RegisterRowForState(string state, int row, int spriteCount);
    Rectangle GetSpriteRectangle(double percentage);
    void SetState(string state);
    string Id { get; set; }
}

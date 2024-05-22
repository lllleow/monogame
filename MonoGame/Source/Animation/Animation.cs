using System;

namespace MonoGame;

public class Animation
{
    public string Id { get; set; }
    public int Duration { get; set; }
    public int SpriteCount { get; set; }

    public Animation(String id, int row, int duration, int spriteCount)
    {
        Id = id;
        Duration = duration;
        SpriteCount = spriteCount;
    }
}

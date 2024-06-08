namespace MonoGame.Source.Systems.Animation;

public class Animation
{
    public string Id { get; set; }
    public int SpritesheetRow { get; set; }
    public int Duration { get; set; }
    public int SpriteCount { get; set; }
    public bool Repeats { get; set; } = false;

    public Animation(string id, bool repeats, int row, int duration, int spriteCount)
    {
        Repeats = repeats;
        Id = id;
        Duration = duration;
        SpriteCount = spriteCount;
        SpritesheetRow = row;
    }
}

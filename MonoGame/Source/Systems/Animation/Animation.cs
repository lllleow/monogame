namespace MonoGame.Source.Systems.Animation;

public class Animation
{
    public Animation(string id, bool repeats, int row, int duration, int spriteCount, bool isDefault = false)
    {
        Repeats = repeats;
        Id = id;
        Duration = duration;
        SpriteCount = spriteCount;
        SpritesheetRow = row;
        IsDefault = isDefault;
    }

    public string Id { get; set; }
    public int SpritesheetRow { get; set; }
    public int Duration { get; set; }
    public int SpriteCount { get; set; }
    public bool Repeats { get; set; }
    public bool IsDefault { get; set; }
}
namespace MonoGame_Common;

public class SharedGlobals
{
    public static int PixelSizeX { get; set; } = 16;
    public static int PixelSizeY { get; set; } = 16;
    public static (int PosX, int PosY) SpawnPosition { get; set; } = new(128, 128);
    public static string ScriptsLocation { get; set; } = @"C:\Users\Leonardo\Documents\Repositories\monogame\MonoGame\Scripts";
}

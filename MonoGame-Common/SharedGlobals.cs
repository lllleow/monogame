namespace MonoGame_Common;

public class SharedGlobals
{
    public static int PixelSizeX { get; set; } = 32;
    public static int PixelSizeY { get; set; } = 32;
    public static (int PosX, int PosY) SpawnPosition { get; set; } = new(128, 128);
}

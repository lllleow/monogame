using Microsoft.Xna.Framework;

namespace MonoGame;

public interface ITile : IInitializable
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SpritesheetName { get; set; }
    public int TextureX { get; set; }
    public int TextureY { get; set; }
    public int SizeX { get; set; }
    public int SizeY { get; set; }
    public Rectangle GetSpriteRectangle()
    {
        return new Rectangle(TextureX * Tile.PixelSizeX, TextureY * Tile.PixelSizeY, SizeX * Tile.PixelSizeX, SizeY * Tile.PixelSizeY);
    }
}

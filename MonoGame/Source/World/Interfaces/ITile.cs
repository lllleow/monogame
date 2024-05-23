using Microsoft.Xna.Framework;

namespace MonoGame;

public interface ITile
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SpritesheetName { get; set; }
    public int TextureX { get; set; }
    public int TextureY { get; set; }
    public int SizeX { get; set; }
    public int SizeY { get; set; }
    public bool IsConnectingTexture { get; set; }
    public int PosX { get; set; }
    public int PosY { get; set; }
    public void UpdateTextureCoordinates();
    public void Initialize(int x, int y);
    void OnNeighborChanged(ITile neighbor, Direction direction);
    public Rectangle GetSpriteRectangle()
    {
        return new Rectangle(TextureX * Tile.PixelSizeX, TextureY * Tile.PixelSizeY, SizeX * Tile.PixelSizeX, SizeY * Tile.PixelSizeY);
    }
}

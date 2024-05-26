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
    public TileTextureType TextureType { get; set; }
    public int PosX { get; set; }
    public int PosY { get; set; }
    public bool Walkable { get; set; }
    public bool DoubleTextureSize { get; set; }
    public void UpdateTextureCoordinates();
    public void Initialize(int x, int y);
    void OnNeighborChanged(ITile neighbor, Direction direction);
    public Rectangle GetSpriteRectangle()
    {
        if (DoubleTextureSize)
        {
            return new Rectangle(TextureX * Tile.PixelSizeX * 2, TextureY * Tile.PixelSizeY * 2, SizeX * Tile.PixelSizeX * 2, SizeY * Tile.PixelSizeY * 2);
        }

        return new Rectangle(TextureX * Tile.PixelSizeX, TextureY * Tile.PixelSizeY, SizeX * Tile.PixelSizeX, SizeY * Tile.PixelSizeY);
    }
}

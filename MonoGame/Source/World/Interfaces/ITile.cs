using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoGame;

public interface ITile
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SpritesheetName { get; set; }
    public int TextureX { get; set; }
    public int TextureY { get; set; }
    float Scale { get; set; }
    public float Opacity { get; set; }
    public int SizeX { get; set; }
    public int SizeY { get; set; }
    public TileTextureType TextureType { get; set; }
    public int PosX { get; set; }
    public int PosY { get; set; }
    public bool DoubleTextureSize { get; set; }
    public bool Walkable { get; set; }
    public List<TileCollisionCriteria> CollisionCriteria { get; set; }
    public void UpdateTextureCoordinates(TileDrawLayer layer);
    public void Initialize(int x, int y);
    void OnNeighborChanged(ITile neighbor, TileDrawLayer layer, Direction direction);
    public Rectangle GetSpriteRectangle()
    {
        if (DoubleTextureSize)
        {
            return new Rectangle(TextureX * Tile.PixelSizeX, TextureY * Tile.PixelSizeY, SizeX * Tile.PixelSizeX, SizeY * Tile.PixelSizeY);
        }

        return new Rectangle(TextureX * Tile.PixelSizeX, TextureY * Tile.PixelSizeY, SizeX * Tile.PixelSizeX, SizeY * Tile.PixelSizeY);
    }
}

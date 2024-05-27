using Microsoft.Xna.Framework;

namespace MonoGame;

/// <summary>
/// Represents a tile in a game world.
/// </summary>
public interface ITile
{
    /// <summary>
    /// Gets or sets the ID of the tile.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the tile.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the name of the spritesheet associated with the tile.
    /// </summary>
    public string SpritesheetName { get; set; }

    /// <summary>
    /// Gets or sets the X coordinate of the tile's texture in the spritesheet.
    /// </summary>
    public int TextureX { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate of the tile's texture in the spritesheet.
    /// </summary>
    public int TextureY { get; set; }

    /// <summary>
    /// Gets or sets the scale of the tile.
    /// </summary>
    float Scale { get; set; }

    /// <summary>
    /// Gets or sets the opacity of the tile.
    /// </summary>
    public float Opacity { get; set; }

    /// <summary>
    /// Gets or sets the width of the tile in Tile.PixelSizeX increments.
    /// </summary>
    public int SizeX { get; set; }

    /// <summary>
    /// Gets or sets the height of the tile in Tile.PixelSizeX increments.
    /// </summary>
    public int SizeY { get; set; }

    /// <summary>
    /// Gets or sets the type of texture used by the tile.
    /// </summary>
    public TileTextureType TextureType { get; set; }

    /// <summary>
    /// Gets or sets the collision type of the tile.
    /// </summary>
    public CollisionMode CollisionMode { get; set; }

    /// <summary>
    /// Gets or sets the spritesheet used for collision masks.
    /// </summary>
    public string CollisionMaskSpritesheetName { get; set; }

    /// <summary>
    /// Gets or sets the X position of the tile in the game world.
    /// </summary>
    public int PosX { get; set; }

    /// <summary>
    /// Gets or sets the Y position of the tile in the game world.
    /// </summary>
    public int PosY { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the tile is walkable.
    /// </summary>
    public bool Walkable { get; set; }

    /// <summary>
    /// Gets or sets an array of connectable tiles.
    /// </summary>
    public string[] ConnectableTiles { get; set; }

    /// <summary>
    /// Updates the texture coordinates of the tile for a specific draw layer.
    /// </summary>
    /// <param name="layer">The draw layer to update the texture coordinates for.</param>
    public void UpdateTextureCoordinates(TileDrawLayer layer);

    /// <summary>
    /// Initializes the tile with the specified X and Y positions.
    /// </summary>
    /// <param name="x">The X position of the tile.</param>
    /// <param name="y">The Y position of the tile.</param>
    public void Initialize(int x, int y);

    /// <summary>
    /// Called when a neighboring tile has changed.
    /// </summary>
    /// <param name="neighbor">The neighboring tile that has changed.</param>
    /// <param name="layer">The draw layer of the neighboring tile.</param>
    /// <param name="direction">The direction of the neighboring tile.</param>
    void OnNeighborChanged(ITile neighbor, TileDrawLayer layer, Direction direction);

    /// <summary>
    /// Gets the rectangle representing the tile's sprite in the spritesheet.
    /// </summary>
    /// <returns>The rectangle representing the tile's sprite.</returns>
    public Rectangle GetSpriteRectangle()
    {
        return new Rectangle(TextureX * Tile.PixelSizeX, TextureY * Tile.PixelSizeY, SizeX * Tile.PixelSizeX, SizeY * Tile.PixelSizeY);
    }

    /// <summary>
    /// Represents a rectangle in a two-dimensional space.
    /// </summary>
    public Rectangle GetRectangle()
    {
        return new Rectangle(PosX, PosY, SizeX * Tile.PixelSizeX, SizeY * Tile.PixelSizeY);
    }
}

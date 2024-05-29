using System;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
namespace MonoGame;

/// <summary>
/// Represents a tile in a game world.
/// </summary>
public class Tile : ITile
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
    /// Gets or sets the X coordinate of the texture in the spritesheet.
    /// </summary>
    public int TextureX { get; set; }

    /// <summary>
    /// Gets or sets the Y coordinate of the texture in the spritesheet.
    /// </summary>
    public int TextureY { get; set; }

    /// <summary>
    /// Gets or sets the current texture index of the tile.
    /// </summary>
    public (int, int) CurrentTextureIndex { get; set; }

    /// <summary>
    /// Gets or sets the texture associated with the tile.
    /// </summary>
    public Texture2D Texture { get; set; }

    /// <summary>
    /// Gets or sets the width of the tile in number of pixels.
    /// </summary>
    public int SizeX { get; set; } = 1;

    /// <summary>
    /// Gets or sets the height of the tile in number of pixels.
    /// </summary>
    public int SizeY { get; set; } = 1;

    /// <summary>
    /// Gets or sets the scale of the tile.
    /// </summary>
    public float Scale { get; set; } = 1;

    /// <summary>
    /// Gets or sets the opacity of the tile.
    /// </summary>
    public float Opacity { get; set; } = 1;

    /// <summary>
    /// Gets or sets the width of the tile in pixels.
    /// </summary>
    public static int PixelSizeX { get; set; } = 16;

    /// <summary>
    /// Gets or sets the height of the tile in pixels.
    /// </summary>
    public static int PixelSizeY { get; set; } = 16;

    /// <summary>
    /// Gets or sets the pixel offset in the X axis of the tile in relation to the grid.
    /// </summary>
    public int PixelOffsetX { get; set; }

    /// <summary>
    /// Gets or sets the pixel offset in the  axis of the tile in relation to the grid.
    /// </summary>
    public int PixelOffsetY { get; set; }

    /// <summary>
    /// Gets or sets the tile processor used for the tile.
    /// </summary>
    public ITileTextureProcessor TextureProcessor { get; set; }

    /// <summary>
    /// Gets or sets the collision type of the tile.
    /// </summary>
    public CollisionMode CollisionMode { get; set; } = CollisionMode.BoundingBox;

    /// <summary>
    /// Gets or sets the spritesheet used for collision masks.
    /// </summary>
    public string CollisionMaskSpritesheetName { get; set; }

    /// <summary>
    /// Gets or sets the X position of the tile.
    /// </summary>
    public int PosX { get; set; }

    /// <summary>
    /// Gets or sets the Y position of the tile.
    /// </summary>
    public int PosY { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the tile is walkable.
    /// </summary>
    public bool Walkable { get; set; } = true;

    /// <summary>
    /// Initializes the tile with the specified position.
    /// </summary>
    /// <param name="x">The X position of the tile.</param>
    /// <param name="y">The Y position of the tile.</param>
    public void Initialize(int x, int y)
    {
        PosX = x;
        PosY = y;
    }

    /// <summary>
    /// Gets or sets an array of connectable tiles.
    /// </summary>
    public string[] ConnectableTiles { get; set; } = System.Array.Empty<string>();

    /// <summary>
    /// Updates the texture coordinates of the tile based on its neighbors.
    /// </summary>
    /// <param name="layer">The draw layer of the tile.</param>
    public void UpdateTextureCoordinates(TileDrawLayer layer)
    {
        TileNeighborConfiguration configuration = GetNeighborConfiguration(layer);
        (int, int) coordinates = TextureProcessor?.Process(configuration) ?? (0, 0);

        CurrentTextureIndex = coordinates;

        TextureX = coordinates.Item1;
        TextureY = coordinates.Item2;
    }

    /// <summary>
    /// Represents the configuration of neighboring tiles for a specific tile.
    /// </summary>
    public TileNeighborConfiguration GetNeighborConfiguration(TileDrawLayer layer)
    {
        ITile left = Globals.world.GetTileAt(layer, PosX - 1, PosY);
        ITile right = Globals.world.GetTileAt(layer, PosX + 1, PosY);
        ITile up = Globals.world.GetTileAt(layer, PosX, PosY - 1);
        ITile down = Globals.world.GetTileAt(layer, PosX, PosY + 1);

        ITile left_top = Globals.world.GetTileAt(layer, PosX - 1, PosY - 1);
        ITile right_top = Globals.world.GetTileAt(layer, PosX + 1, PosY - 1);
        ITile left_bottom = Globals.world.GetTileAt(layer, PosX - 1, PosY + 1);
        ITile right_bottom = Globals.world.GetTileAt(layer, PosX + 1, PosY + 1);

        return new TileNeighborConfiguration(this, left, right, up, down, left_top, right_top, left_bottom, right_bottom);
    }

    /// <summary>
    /// Called when a neighbor tile has changed.
    /// </summary>
    /// <param name="neighbor">The neighbor tile that has changed.</param>
    /// <param name="layer">The draw layer of the tile.</param>
    /// <param name="direction">The direction of the neighbor tile.</param>
    public void OnNeighborChanged(ITile neighbor, TileDrawLayer layer, Direction direction)
    {
        if (TextureProcessor != null)
        {
            UpdateTextureCoordinates(layer);
        }
    }
}

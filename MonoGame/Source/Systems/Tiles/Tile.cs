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
    public Vector2 CurrentTextureIndex { get; set; }

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
    /// Gets or sets the width of a pixel in the tile.
    /// </summary>
    public static int PixelSizeX { get; set; } = 16;

    /// <summary>
    /// Gets or sets the height of a pixel in the tile.
    /// </summary>
    public static int PixelSizeY { get; set; } = 16;

    /// <summary>
    /// Gets or sets the type of texture for the tile.
    /// </summary>
    public TileTextureType TextureType { get; set; } = TileTextureType.Basic;

    /// <summary>
    /// Gets or sets the collision type of the tile.
    /// </summary>
    public CollisionMode CollisionType { get; set; } = CollisionMode.BoundingBox;

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
    /// Updates the texture coordinates of the tile based on its neighbors.
    /// </summary>
    /// <param name="layer">The draw layer of the tile.</param>
    public void UpdateTextureCoordinates(TileDrawLayer layer)
    {
        ITile left = Globals.world.GetTileAt(layer, PosX - 1, PosY);
        ITile right = Globals.world.GetTileAt(layer, PosX + 1, PosY);
        ITile up = Globals.world.GetTileAt(layer, PosX, PosY - 1);
        ITile down = Globals.world.GetTileAt(layer, PosX, PosY + 1);

        ITile left_top = Globals.world.GetTileAt(layer, PosX - 1, PosY - 1);
        ITile right_top = Globals.world.GetTileAt(layer, PosX + 1, PosY - 1);
        ITile left_bottom = Globals.world.GetTileAt(layer, PosX - 1, PosY + 1);
        ITile right_bottom = Globals.world.GetTileAt(layer, PosX + 1, PosY + 1);

        bool leftIsSame = IsSameType(left);
        bool rightIsSame = IsSameType(right);
        bool upIsSame = IsSameType(up);
        bool downIsSame = IsSameType(down);

        bool left_topIsSame = IsSameType(left_top);
        bool right_topIsSame = IsSameType(right_top);
        bool left_bottomIsSame = IsSameType(left_bottom);
        bool right_bottomIsSame = IsSameType(right_bottom);

        Vector2 coordinates = new Vector2(0, 0);

        if (TextureType == TileTextureType.CompleteConnecting)
        {
            if (leftIsSame && rightIsSame && upIsSame && downIsSame)
            {
                if (!left_bottomIsSame && !right_bottomIsSame)
                {
                    coordinates = new Vector2(4, 1);
                }
                else if (!left_bottomIsSame)
                {
                    coordinates = new Vector2(7, 2);
                }
                else if (!right_topIsSame && !right_bottomIsSame)
                {
                    coordinates = new Vector2(7, 1);
                }
                else if (!left_topIsSame && !left_bottomIsSame)
                {
                    coordinates = new Vector2(7, 2);
                }
                else
                {
                    coordinates = new Vector2(1, 1);
                }
            }
            else if (leftIsSame && rightIsSame && upIsSame)
            {
                coordinates = new Vector2(1, 2);
            }
            else if (leftIsSame && rightIsSame && downIsSame)
            {
                if (!right_bottomIsSame && !left_bottomIsSame)
                {
                    coordinates = new Vector2(4, 2);
                }
                else if (!right_bottomIsSame)
                {
                    coordinates = new Vector2(6, 2);
                }
                else if (!left_bottomIsSame)
                {
                    coordinates = new Vector2(6, 1);
                }
                else
                {
                    coordinates = new Vector2(1, 0);
                }
            }
            else if (upIsSame && downIsSame && rightIsSame)
            {
                if (!right_bottomIsSame)
                {
                    coordinates = new Vector2(5, 2);
                }
                else
                {
                    coordinates = new Vector2(0, 1);
                }
            }
            else if (upIsSame && downIsSame && leftIsSame)
            {
                if (!left_bottomIsSame)
                {
                    coordinates = new Vector2(5, 1);
                }
                else
                {
                    coordinates = new Vector2(2, 1);
                }
            }
            else if (leftIsSame && rightIsSame && !upIsSame && !downIsSame)
            {
                coordinates = new Vector2(5, 0);
            }
            else if (upIsSame && downIsSame)
            {
                coordinates = new Vector2(3, 1);
            }
            else if (leftIsSame && upIsSame)
            {
                coordinates = new Vector2(2, 2);
            }
            else if (rightIsSame && upIsSame)
            {
                coordinates = new Vector2(0, 2);
            }
            else if (leftIsSame && downIsSame)
            {
                if (!left_bottomIsSame)
                {
                    coordinates = new Vector2(9, 2);
                }
                else
                {
                    coordinates = new Vector2(2, 0);
                }
            }
            else if (rightIsSame && downIsSame)
            {
                if (!right_bottomIsSame)
                {
                    coordinates = new Vector2(9, 0);
                }
                else
                {
                    coordinates = new Vector2(0, 0);
                }
            }
            else if (leftIsSame)
            {
                coordinates = new Vector2(6, 0);
            }
            else if (rightIsSame)
            {
                coordinates = new Vector2(4, 0);
            }
            else if (upIsSame)
            {
                coordinates = new Vector2(3, 2);
            }
            else if (downIsSame)
            {
                coordinates = new Vector2(3, 0);
            }
            else
            {
                coordinates = new Vector2(7, 0);
            }
            // ... (omitted for brevity)
        }
        else if (TextureType == TileTextureType.SimpleConnecting)
        {
            if (leftIsSame && rightIsSame && upIsSame && downIsSame)
            {
                coordinates = new Vector2(1, 1);
            }
            else if (leftIsSame && rightIsSame && upIsSame)
            {
                coordinates = new Vector2(1, 2);
            }
            else if (leftIsSame && rightIsSame && downIsSame)
            {
                coordinates = new Vector2(1, 0);
            }
            else if (upIsSame && downIsSame && rightIsSame)
            {
                coordinates = new Vector2(0, 1);
            }
            else if (upIsSame && downIsSame && leftIsSame)
            {
                coordinates = new Vector2(2, 1);
            }
            else if (leftIsSame && rightIsSame && !upIsSame && !downIsSame)
            {
                coordinates = new Vector2(5, 0);
            }
            else if (upIsSame && downIsSame)
            {
                coordinates = new Vector2(3, 1);
            }
            else if (leftIsSame && upIsSame)
            {
                coordinates = new Vector2(2, 2);
            }
            else if (rightIsSame && upIsSame)
            {
                coordinates = new Vector2(0, 2);
            }
            else if (leftIsSame && downIsSame)
            {
                coordinates = new Vector2(2, 0);
            }
            else if (rightIsSame && downIsSame)
            {
                coordinates = new Vector2(0, 0);
            }
            else if (leftIsSame)
            {
                coordinates = new Vector2(6, 0);
            }
            else if (rightIsSame)
            {
                coordinates = new Vector2(4, 0);
            }
            else if (upIsSame)
            {
                coordinates = new Vector2(3, 2);
            }
            else if (downIsSame)
            {
                coordinates = new Vector2(3, 0);
            }
            else
            {
                coordinates = new Vector2(7, 0);
            }
        }

        CurrentTextureIndex = coordinates;

        TextureX = (int)coordinates.X;
        TextureY = (int)coordinates.Y;
    }

    /// <summary>
    /// Checks if the specified tile is of the same type as the current tile.
    /// </summary>
    /// <param name="tile">The tile to compare.</param>
    /// <returns><c>true</c> if the tiles are of the same type; otherwise, <c>false</c>.</returns>
    private bool IsSameType(ITile tile)
    {
        return tile != null && tile.GetType() == GetType();
    }

    /// <summary>
    /// Called when a neighbor tile has changed.
    /// </summary>
    /// <param name="neighbor">The neighbor tile that has changed.</param>
    /// <param name="layer">The draw layer of the tile.</param>
    /// <param name="direction">The direction of the neighbor tile.</param>
    public void OnNeighborChanged(ITile neighbor, TileDrawLayer layer, Direction direction)
    {
        if (TextureType != TileTextureType.Basic)
        {
            UpdateTextureCoordinates(layer);
        }
    }
}

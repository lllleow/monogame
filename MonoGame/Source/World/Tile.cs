using System;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame;

public class Tile : ITile
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string SpritesheetName { get; set; }
    public int TextureX { get; set; }
    public int TextureY { get; set; }
    public Texture2D Texture { get; set; }
    public int SizeX { get; set; } = 1;
    public int SizeY { get; set; } = 1;
    public bool IsConnectingTexture { get; set; } = false;
    public static int PixelSizeX { get; set; } = 16;
    public static int PixelSizeY { get; set; } = 16;
    public int PosX { get; set; }
    public int PosY { get; set; }
    public bool DoubleTextureSize { get; set; } = false;
    public bool Walkable { get; set; }

    public void Initialize(int x, int y)
    {
        PosX = x;
        PosY = y;
    }

    public void UpdateTextureCoordinates()
    {
        ITile left = Globals.world.GetTileAt(1, PosX - 1, PosY);
        ITile right = Globals.world.GetTileAt(1, PosX + 1, PosY);
        ITile up = Globals.world.GetTileAt(1, PosX, PosY - 1);
        ITile down = Globals.world.GetTileAt(1, PosX, PosY + 1);

        ITile left_top = Globals.world.GetTileAt(1, PosX - 1, PosY - 1);
        ITile right_top = Globals.world.GetTileAt(1, PosX + 1, PosY - 1);
        ITile left_bottom = Globals.world.GetTileAt(1, PosX - 1, PosY + 1);
        ITile right_bottom = Globals.world.GetTileAt(1, PosX + 1, PosY + 1);

        bool leftIsSame = IsSameType(left);
        bool rightIsSame = IsSameType(right);
        bool upIsSame = IsSameType(up);
        bool downIsSame = IsSameType(down);

        bool left_topIsSame = IsSameType(left_top);
        bool right_topIsSame = IsSameType(right_top);
        bool left_bottomIsSame = IsSameType(left_bottom);
        bool right_bottomIsSame = IsSameType(right_bottom);

        Vector2 coordinates;

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

        TextureX = (int)coordinates.X;
        TextureY = (int)coordinates.Y;
    }

    private bool IsSameType(ITile tile)
    {
        return tile != null && tile.GetType() == this.GetType();
    }

    public void OnNeighborChanged(ITile neighbor, Direction direction)
    {
        if (IsConnectingTexture)
        {
            UpdateTextureCoordinates();
        }
        else
        {
            TextureX = 1;
            TextureY = 1;
        }
    }
}

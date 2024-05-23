using System;
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

    public void Initialize(int x, int y)
    {
        PosX = x;
        PosY = y;
    }

    public void UpdateTextureCoordinates()
    {
        ITile left = Globals.world.GetTileAt(1, PosX - 1, PosY);
        ITile left_top = Globals.world.GetTileAt(1, PosX - 1, PosY + 1);
        ITile left_bottom = Globals.world.GetTileAt(1, PosX - 1, PosY - 1);
        ITile right = Globals.world.GetTileAt(1, PosX + 1, PosY);
        ITile right_top = Globals.world.GetTileAt(1, PosX + 1, PosY + 1);
        ITile right_bottom = Globals.world.GetTileAt(1, PosX + 1, PosY - 1);
        ITile up = Globals.world.GetTileAt(1, PosX, PosY - 1);
        ITile down = Globals.world.GetTileAt(1, PosX, PosY + 1);

        if (!IsSameType(left) && !IsSameType(right) && !IsSameType(up) && !IsSameType(down))
        {
            TextureX = 3;
            TextureY = 2;
        }

        if (!IsSameType(left) && IsSameType(right) && IsSameType(up) && IsSameType(down))
        {
            TextureX = 0;
            TextureY = 1;
        }

        if (IsSameType(left) && IsSameType(right) && IsSameType(up) && IsSameType(down))
        {
            TextureX = 1;
            TextureY = 1;
        }

        if (IsSameType(left) && !IsSameType(right) && !IsSameType(up) && !IsSameType(down))
        {
            TextureX = 5;
            TextureY = 2;
        }

        if (!IsSameType(left) && IsSameType(right) && !IsSameType(up) && !IsSameType(down))
        {
            TextureX = 4;
            TextureY = 2;
        }

        if (!IsSameType(left) && !IsSameType(right) && IsSameType(up) && !IsSameType(down))
        {
            TextureX = 3;
            TextureY = 1;
        }

        if (!IsSameType(left) && !IsSameType(right) && !IsSameType(up) && IsSameType(down))
        {
            TextureX = 3;
            TextureY = 0;
        }

        if (!IsSameType(left) && IsSameType(right) && !IsSameType(up) && IsSameType(down))
        {
            TextureX = 0;
            TextureY = 0;
        }

        if (IsSameType(left) && !IsSameType(right) && !IsSameType(up) && IsSameType(down))
        {
            TextureX = 2;
            TextureY = 0;
        }

        if (!IsSameType(left) && IsSameType(right) && IsSameType(up) && !IsSameType(down))
        {
            TextureX = 0;
            TextureY = 2;
        }

        if (IsSameType(left) && !IsSameType(right) && IsSameType(up) && !IsSameType(down))
        {
            TextureX = 2;
            TextureY = 2;
        }

        if (IsSameType(left) && IsSameType(right) && !IsSameType(up) && IsSameType(down))
        {
            TextureX = 1;
            TextureY = 0;
        }

        if (IsSameType(left) && IsSameType(right) && IsSameType(up) && !IsSameType(down))
        {
            TextureX = 1;
            TextureY = 2;
        }

        if (IsSameType(left) && !IsSameType(right) && IsSameType(up) && IsSameType(down))
        {
            TextureX = 2;
            TextureY = 1;
        }

        if (IsSameType(left) && IsSameType(right) && !IsSameType(up) && !IsSameType(down))
        {
            TextureX = 6;
            TextureY = 2;
        }

        if (!IsSameType(left) && !IsSameType(right) && IsSameType(up) && IsSameType(down))
        {
            TextureX = 6;
            TextureY = 1;
        }
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

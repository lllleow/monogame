using System.Drawing;
using System.Numerics;
using MonoGame_Common.States;
using MonoGame_Common.Systems.Tiles.Interfaces;

namespace MonoGame_Common;

public class PositionedTileHelper
{
    public int PosX { get; set; }
    public int PosY { get; set; }
    public TileState Tile { get; set; }
    public ChunkState Chunk { get; set; }

    public PositionedTileHelper(TileState tile, ChunkState chunkState, int posX, int posY)
    {
        Tile = tile;
        Chunk = chunkState;
        PosX = posX;
        PosY = posY;
    }

    public Rectangle GetTileRect()
    {
        Vector2 worldPosition = Chunk.GetWorldPosition(PosX, PosY);
        CommonTile? tile = Tile.GetCommonTile();
        if (tile == null) return Rectangle.Empty;
        return new Rectangle((int)worldPosition.X * SharedGlobals.PixelSizeX, (int)worldPosition.Y * SharedGlobals.PixelSizeY, tile.TileSizeX * SharedGlobals.PixelSizeX, tile.TileSizeY * SharedGlobals.PixelSizeY);
    }
}

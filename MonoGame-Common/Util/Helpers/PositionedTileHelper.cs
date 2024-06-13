using System.Drawing;
using System.Numerics;
using MonoGame_Common.States;

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
        return new Rectangle((int)worldPosition.X, (int)worldPosition.Y, Tile.GetCommonTile().TileSizeX * SharedGlobals.PixelSizeX, Tile.GetCommonTile().TileSizeY * SharedGlobals.PixelSizeY);
    }
}

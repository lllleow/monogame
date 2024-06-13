using MonoGame_Common.Systems.Tiles.Interfaces;

namespace MonoGame_Common.Util.Tile;

public class TileNeighborConfiguration
{
    public TileNeighborConfiguration(CommonTile? tile, CommonTile? left, CommonTile? right, CommonTile? up, CommonTile? down, CommonTile? leftTop, CommonTile? rightTop, CommonTile? leftBottom, CommonTile? rightBottom)
    {
        Center = tile;
        Left = left;
        Right = right;
        Up = up;
        Down = down;
        LeftTop = leftTop;
        RightTop = rightTop;
        LeftBottom = leftBottom;
        RightBottom = rightBottom;
    }

    public CommonTile? Center { get; set; }
    public CommonTile? Left { get; set; }
    public CommonTile? Right { get; set; }
    public CommonTile? Up { get; set; }
    public CommonTile? Down { get; set; }
    public CommonTile? LeftTop { get; set; }
    public CommonTile? RightTop { get; set; }
    public CommonTile? LeftBottom { get; set; }
    public CommonTile? RightBottom { get; set; }
}
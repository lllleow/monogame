using MonoGame.Source.Systems.Tiles.Interfaces;

namespace MonoGame.Source.Systems.Tiles.Utils;

public class TileNeighborConfiguration
{
    public ITile Center;

    public ITile Left;
    public ITile Right;
    public ITile Up;
    public ITile Down;

    public ITile LeftTop;
    public ITile RightTop;
    public ITile LeftBottom;
    public ITile RightBottom;

    public TileNeighborConfiguration(ITile tile, ITile left, ITile right, ITile up, ITile down, ITile leftTop, ITile rightTop, ITile leftBottom, ITile rightBottom)
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
}

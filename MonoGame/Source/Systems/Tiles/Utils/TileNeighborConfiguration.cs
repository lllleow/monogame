using MonoGame.Source.Systems.Tiles.Interfaces;

namespace MonoGame.Source.Systems.Tiles.Utils;

public class TileNeighborConfiguration
{
    public ITile Center { get; set; }

    public ITile Left { get; set; }
    public ITile Right { get; set; }
    public ITile Up { get; set; }
    public ITile Down { get; set; }

    public ITile LeftTop { get; set; }
    public ITile RightTop { get; set; }
    public ITile LeftBottom { get; set; }
    public ITile RightBottom { get; set; }

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

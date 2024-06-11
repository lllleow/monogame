using MonoGame.Source.Systems.Tiles.Interfaces;

namespace MonoGame.Source.Systems.Tiles.Utils;

public class TileNeighborConfiguration
{
    public TileNeighborConfiguration(Tile tile, Tile left, Tile right, Tile up, Tile down, Tile leftTop, Tile rightTop, Tile leftBottom, Tile rightBottom)
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

    public Tile Center { get; set; }

    public Tile Left { get; set; }
    public Tile Right { get; set; }
    public Tile Up { get; set; }
    public Tile Down { get; set; }

    public Tile LeftTop { get; set; }
    public Tile RightTop { get; set; }
    public Tile LeftBottom { get; set; }
    public Tile RightBottom { get; set; }
}
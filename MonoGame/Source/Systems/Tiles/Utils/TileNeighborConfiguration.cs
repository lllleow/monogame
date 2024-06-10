namespace MonoGame.Source.Systems.Tiles.Utils;

public class TileNeighborConfiguration
{
    public TileNeighborConfiguration(PlacedTile tile, PlacedTile left, PlacedTile right, PlacedTile up, PlacedTile down, PlacedTile leftTop, PlacedTile rightTop, PlacedTile leftBottom, PlacedTile rightBottom)
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

    public PlacedTile Center { get; set; }

    public PlacedTile Left { get; set; }
    public PlacedTile Right { get; set; }
    public PlacedTile Up { get; set; }
    public PlacedTile Down { get; set; }

    public PlacedTile LeftTop { get; set; }
    public PlacedTile RightTop { get; set; }
    public PlacedTile LeftBottom { get; set; }
    public PlacedTile RightBottom { get; set; }
}
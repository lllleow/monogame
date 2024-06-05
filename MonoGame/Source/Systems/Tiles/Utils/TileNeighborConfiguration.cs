namespace MonoGame.Source.Systems.Tiles.Utils;

/// <summary>
/// Represents the configuration of neighboring tiles for a specific tile.
/// </summary>
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

    /// <summary>
    /// Initializes a new instance of the <see cref="TileNeighborConfiguration"/> class.
    /// </summary>
    /// <param name="tile">The center tile.</param>
    /// <param name="left">The left tile.</param>
    /// <param name="right">The right tile.</param>
    /// <param name="up">The top tile.</param>
    /// <param name="down">The bottom tile.</param>
    /// <param name="leftTop">The top-left tile.</param>
    /// <param name="rightTop">The top-right tile.</param>
    /// <param name="leftBottom">The bottom-left tile.</param>
    /// <param name="rightBottom">The bottom-right tile.</param>
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

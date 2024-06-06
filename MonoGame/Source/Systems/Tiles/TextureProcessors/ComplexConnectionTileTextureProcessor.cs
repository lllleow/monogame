using MonoGame.Source.Systems.Tiles.Utils;
using MonoGame.Source.Util.Enum;

namespace MonoGame.Source.Systems.Tiles.TextureProcessors;

public class ComplexConnectionTileTextureProcessor : TileTextureProcessor
{
    public static ComplexConnectionTileTextureProcessor Instance = new();

    public override (int TextureCoordinateX, int TextureCoordinateY) Process(TileNeighborConfiguration configuration)
    {
        var leftCanConnect = CanConnect(configuration, Direction.Left);
        var rightCanConnect = CanConnect(configuration, Direction.Right);
        var upCanConnect = CanConnect(configuration, Direction.Up);
        var downCanConnect = CanConnect(configuration, Direction.Down);

        var left_topCanConnect = CanConnect(configuration, Direction.LeftUp);
        var right_topCanConnect = CanConnect(configuration, Direction.RightUp);
        var left_bottomCanConnect = CanConnect(configuration, Direction.LeftDown);
        var right_bottomCanConnect = CanConnect(configuration, Direction.RightDown);

        if (leftCanConnect && rightCanConnect && upCanConnect && downCanConnect)
        {
            return !left_bottomCanConnect && !right_bottomCanConnect
                ? (4, 1)
                : !left_bottomCanConnect
                    ? (7, 2)
                    : !right_topCanConnect && !right_bottomCanConnect ? (7, 1) : !left_topCanConnect && !left_bottomCanConnect ? (7, 2) : (1, 1);
        }
        else if (leftCanConnect && rightCanConnect && upCanConnect)
        {
            return (1, 2);
        }
        else if (leftCanConnect && rightCanConnect && downCanConnect)
        {
            return !right_bottomCanConnect && !left_bottomCanConnect
                ? (4, 2)
                : !right_bottomCanConnect ? (6, 2) : !left_bottomCanConnect ? (6, 1) : (1, 0);
        }
        else if (upCanConnect && downCanConnect && rightCanConnect)
        {
            return !right_bottomCanConnect ? (5, 2) : (0, 1);
        }
        else if (upCanConnect && downCanConnect && leftCanConnect)
        {
            return !left_bottomCanConnect ? (5, 1) : (2, 1);
        }
        else if (leftCanConnect && rightCanConnect && !upCanConnect && !downCanConnect)
        {
            return (5, 0);
        }
        else if (upCanConnect && downCanConnect)
        {
            return (3, 1);
        }
        else if (leftCanConnect && upCanConnect)
        {
            return (2, 2);
        }
        else if (rightCanConnect && upCanConnect)
        {
            return (0, 2);
        }
        else if (leftCanConnect && downCanConnect)
        {
            return !left_bottomCanConnect ? (9, 2) : (2, 0);
        }
        else if (rightCanConnect && downCanConnect)
        {
            return !right_bottomCanConnect ? (9, 0) : (0, 0);
        }
        else
        {
            return leftCanConnect ? (6, 0) : rightCanConnect ? (4, 0) : upCanConnect ? (3, 2) : downCanConnect ? (3, 0) : (7, 0);
        }
    }
}

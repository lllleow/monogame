using MonoGame_Common.Util.Enum;

namespace MonoGame_Common.Util.Tile.TextureProcessors;

public class ComplexConnectionTileTextureProcessor : TileTextureProcessor
{
    public static ComplexConnectionTileTextureProcessor Instance { get; set; } = new();

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

        return leftCanConnect && rightCanConnect && upCanConnect && downCanConnect
            ? !left_bottomCanConnect && !right_bottomCanConnect
                ? (4, 1)
                : !left_bottomCanConnect
                    ? (7, 2)
                    : !right_topCanConnect && !right_bottomCanConnect
                        ? (7, 1)
                        : !left_topCanConnect && !left_bottomCanConnect
                            ? (7, 2)
                            : (1, 1)
            : leftCanConnect && rightCanConnect && upCanConnect
                ? (1, 2)
                : leftCanConnect && rightCanConnect && downCanConnect
                    ? !right_bottomCanConnect && !left_bottomCanConnect
                        ? (4, 2)
                        : !right_bottomCanConnect
                            ? (6, 2)
                            : !left_bottomCanConnect
                                ? (6, 1)
                                : (1, 0)
                    : upCanConnect && downCanConnect && rightCanConnect
                        ? !right_bottomCanConnect ? (5, 2) : (0, 1)
                        : upCanConnect && downCanConnect && leftCanConnect
                            ? !left_bottomCanConnect ? (5, 1) : (2, 1)
                            : leftCanConnect && rightCanConnect && !upCanConnect && !downCanConnect
                                ? (5, 0)
                                : upCanConnect && downCanConnect
                                    ? (3, 1)
                                    : leftCanConnect && upCanConnect
                                        ? (2, 2)
                                        : rightCanConnect && upCanConnect
                                            ? (0, 2)
                                            : leftCanConnect && downCanConnect
                                                ? !left_bottomCanConnect ? (9, 2) : (2, 0)
                                                : rightCanConnect && downCanConnect
                                                    ? !right_bottomCanConnect ? (9, 0) : (0, 0)
                                                    : leftCanConnect
                                                        ? (6, 0)
                                                        : rightCanConnect
                                                            ? (4, 0)
                                                            : upCanConnect
                                                                ? (3, 2)
                                                                : downCanConnect
                                                                    ? (3, 0)
                                                                    : (7, 0);
    }
}
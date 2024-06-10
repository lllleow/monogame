using MonoGame_Common.Util.Enum;
using MonoGame.Source.Systems.Tiles.Utils;

namespace MonoGame.Source.Systems.Tiles.TextureProcessors;

public class SimpleConnectionTileTextureProcessor : TileTextureProcessor
{
    public static SimpleConnectionTileTextureProcessor Instance { get; set; } = new();

    public override (int TextureCoordinateX, int TextureCoordinateY) Process(TileNeighborConfiguration configuration)
    {
        var leftCanConnect = CanConnect(configuration, Direction.Left);
        var rightCanConnect = CanConnect(configuration, Direction.Right);
        var upCanConnect = CanConnect(configuration, Direction.Up);
        var downCanConnect = CanConnect(configuration, Direction.Down);

        return leftCanConnect && rightCanConnect && upCanConnect && downCanConnect
            ? (1, 1)
            : leftCanConnect && rightCanConnect && upCanConnect
                ? (1, 2)
                : leftCanConnect && rightCanConnect && downCanConnect
                    ? (1, 0)
                    : upCanConnect && downCanConnect && rightCanConnect
                        ? (0, 1)
                        : upCanConnect && downCanConnect && leftCanConnect
                            ? (2, 1)
                            : leftCanConnect && rightCanConnect && !upCanConnect && !downCanConnect
                                ? (5, 0)
                                : upCanConnect && downCanConnect
                                    ? (3, 1)
                                    : leftCanConnect && upCanConnect
                                        ? (2, 2)
                                        : rightCanConnect && upCanConnect
                                            ? (0, 2)
                                            : leftCanConnect && downCanConnect
                                                ? (2, 0)
                                                : rightCanConnect && downCanConnect
                                                    ? (0, 0)
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
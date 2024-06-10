using MonoGame_Common.Util.Enum;
using MonoGame.Source.Systems.Tiles.Utils;

namespace MonoGame.Source.Systems.Tiles.TextureProcessors;

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
            ? ((int TextureCoordinateX, int TextureCoordinateY))(!left_bottomCanConnect && !right_bottomCanConnect
                ? (4, 1)
                : !left_bottomCanConnect
                    ? (7, 2)
                    : !right_topCanConnect && !right_bottomCanConnect ? (7, 1) : !left_topCanConnect && !left_bottomCanConnect ? (7, 2) : (1, 1))
            : leftCanConnect && rightCanConnect && upCanConnect
                ? ((int TextureCoordinateX, int TextureCoordinateY))(1, 2)
                : leftCanConnect && rightCanConnect && downCanConnect
                            ? ((int TextureCoordinateX, int TextureCoordinateY))(!right_bottomCanConnect && !left_bottomCanConnect
                                        ? (4, 2)
                                        : !right_bottomCanConnect ? (6, 2) : !left_bottomCanConnect ? (6, 1) : (1, 0))
                            : upCanConnect && downCanConnect && rightCanConnect
                                        ? ((int TextureCoordinateX, int TextureCoordinateY))(!right_bottomCanConnect ? (5, 2) : (0, 1))
                                        : upCanConnect && downCanConnect && leftCanConnect
                                                    ? ((int TextureCoordinateX, int TextureCoordinateY))(!left_bottomCanConnect ? (5, 1) : (2, 1))
                                                    : leftCanConnect && rightCanConnect && !upCanConnect && !downCanConnect
                                                                ? ((int TextureCoordinateX, int TextureCoordinateY))(5, 0)
                                                                : upCanConnect && downCanConnect
                                                                            ? ((int TextureCoordinateX, int TextureCoordinateY))(3, 1)
                                                                            : leftCanConnect && upCanConnect
                                                                                        ? ((int TextureCoordinateX, int TextureCoordinateY))(2, 2)
                                                                                        : rightCanConnect && upCanConnect
                                                                                                    ? ((int TextureCoordinateX, int TextureCoordinateY))(0, 2)
                                                                                                    : leftCanConnect && downCanConnect
                                                                                                                ? ((int TextureCoordinateX, int TextureCoordinateY))(!left_bottomCanConnect ? (9, 2) : (2, 0))
                                                                                                                : rightCanConnect && downCanConnect
                                                                                                                            ? ((int TextureCoordinateX, int TextureCoordinateY))(!right_bottomCanConnect ? (9, 0) : (0, 0))
                                                                                                                            : ((int TextureCoordinateX, int TextureCoordinateY))(leftCanConnect ? (6, 0) : rightCanConnect ? (4, 0) : upCanConnect ? (3, 2) : downCanConnect ? (3, 0) : (7, 0));
    }
}

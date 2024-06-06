using MonoGame.Source.Systems.Tiles.Utils;
using MonoGame.Source.Util.Enum;

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

        if (leftCanConnect && rightCanConnect && upCanConnect && downCanConnect)
        {
            return (1, 1);
        }
        else if (leftCanConnect && rightCanConnect && upCanConnect)
        {
            return (1, 2);
        }
        else if (leftCanConnect && rightCanConnect && downCanConnect)
        {
            return (1, 0);
        }
        else if (upCanConnect && downCanConnect && rightCanConnect)
        {
            return (0, 1);
        }
        else if (upCanConnect && downCanConnect && leftCanConnect)
        {
            return (2, 1);
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
            return (2, 0);
        }
        else if (rightCanConnect && downCanConnect)
        {
            return (0, 0);
        }
        else
        {
            return leftCanConnect ? (6, 0) : rightCanConnect ? (4, 0) : upCanConnect ? (3, 2) : downCanConnect ? (3, 0) : (7, 0);
        }
    }
}

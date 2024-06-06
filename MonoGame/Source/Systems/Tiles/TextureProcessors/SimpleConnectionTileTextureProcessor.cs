using MonoGame.Source.Systems.Tiles.Utils;

namespace MonoGame;

public class SimpleConnectionTileTextureProcessor : TileTextureProcessor
{

    public static SimpleConnectionTileTextureProcessor instance = new SimpleConnectionTileTextureProcessor();

    public override (int, int) Process(TileNeighborConfiguration configuration)
    {
        bool leftCanConnect = CanConnect(configuration, Direction.Left);
        bool rightCanConnect = CanConnect(configuration, Direction.Right);
        bool upCanConnect = CanConnect(configuration, Direction.Up);
        bool downCanConnect = CanConnect(configuration, Direction.Down);

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
        else if (leftCanConnect)
        {
            return (6, 0);
        }
        else if (rightCanConnect)
        {
            return (4, 0);
        }
        else if (upCanConnect)
        {
            return (3, 2);
        }
        else if (downCanConnect)
        {
            return (3, 0);
        }
        else
        {
            return (7, 0);
        }
    }
}

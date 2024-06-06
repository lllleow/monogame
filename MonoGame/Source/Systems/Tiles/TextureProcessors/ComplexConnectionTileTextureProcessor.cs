using MonoGame.Source.Systems.Tiles.Utils;

namespace MonoGame;

public class ComplexConnectionTileTextureProcessor : TileTextureProcessor
{

    public static ComplexConnectionTileTextureProcessor instance = new ComplexConnectionTileTextureProcessor();

    public override (int, int) Process(TileNeighborConfiguration configuration)
    {
        bool leftCanConnect = CanConnect(configuration, Direction.Left);
        bool rightCanConnect = CanConnect(configuration, Direction.Right);
        bool upCanConnect = CanConnect(configuration, Direction.Up);
        bool downCanConnect = CanConnect(configuration, Direction.Down);

        bool left_topCanConnect = CanConnect(configuration, Direction.LeftUp);
        bool right_topCanConnect = CanConnect(configuration, Direction.RightUp);
        bool left_bottomCanConnect = CanConnect(configuration, Direction.LeftDown);
        bool right_bottomCanConnect = CanConnect(configuration, Direction.RightDown);

        if (leftCanConnect && rightCanConnect && upCanConnect && downCanConnect)
        {
            if (!left_bottomCanConnect && !right_bottomCanConnect)
            {
                return (4, 1);
            }
            else if (!left_bottomCanConnect)
            {
                return (7, 2);
            }
            else if (!right_topCanConnect && !right_bottomCanConnect)
            {
                return (7, 1);
            }
            else if (!left_topCanConnect && !left_bottomCanConnect)
            {
                return (7, 2);
            }
            else
            {
                return (1, 1);
            }
        }
        else if (leftCanConnect && rightCanConnect && upCanConnect)
        {
            return (1, 2);
        }
        else if (leftCanConnect && rightCanConnect && downCanConnect)
        {
            if (!right_bottomCanConnect && !left_bottomCanConnect)
            {
                return (4, 2);
            }
            else if (!right_bottomCanConnect)
            {
                return (6, 2);
            }
            else if (!left_bottomCanConnect)
            {
                return (6, 1);
            }
            else
            {
                return (1, 0);
            }
        }
        else if (upCanConnect && downCanConnect && rightCanConnect)
        {
            if (!right_bottomCanConnect)
            {
                return (5, 2);
            }
            else
            {
                return (0, 1);
            }
        }
        else if (upCanConnect && downCanConnect && leftCanConnect)
        {
            if (!left_bottomCanConnect)
            {
                return (5, 1);
            }
            else
            {
                return (2, 1);
            }
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
            if (!left_bottomCanConnect)
            {
                return (9, 2);
            }
            else
            {
                return (2, 0);
            }
        }
        else if (rightCanConnect && downCanConnect)
        {
            if (!right_bottomCanConnect)
            {
                return (9, 0);
            }
            else
            {
                return (0, 0);
            }
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


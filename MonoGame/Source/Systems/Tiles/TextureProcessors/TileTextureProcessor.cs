using System;
using System.Collections.Generic;
using MonoGame.Source.Systems.Tiles.Utils;

namespace MonoGame;

public class TileTextureProcessor : ITileTextureProcessor
{

    public virtual (int, int) Process(TileNeighborConfiguration configuration)
    {
        throw new NotImplementedException();
    }

    public bool CanConnect(TileNeighborConfiguration configuration, Direction direction)
    {
        return IsOfSameType(configuration, direction) || IsWhitelisted(configuration, direction);
    }

    public bool IsOfSameType(TileNeighborConfiguration configuration, Direction direction)
    {
        if (configuration.Center is null)
            return false;

        switch (direction)
        {
            case Direction.Left:
                return configuration.Left?.GetType() == configuration.Center.GetType();
            case Direction.Right:
                return configuration.Right?.GetType() == configuration.Center.GetType();
            case Direction.Up:
                return configuration.Up?.GetType() == configuration.Center.GetType();
            case Direction.Down:
                return configuration.Down?.GetType() == configuration.Center.GetType();
            case Direction.LeftUp:
                return configuration.LeftTop?.GetType() == configuration.Center.GetType();
            case Direction.RightUp:
                return configuration.RightTop?.GetType() == configuration.Center.GetType();
            case Direction.LeftDown:
                return configuration.LeftBottom?.GetType() == configuration.Center.GetType();
            case Direction.RightDown:
                return configuration.RightBottom?.GetType() == configuration.Center.GetType();
            default:
                return false;
        }
    }

    public bool IsWhitelisted(TileNeighborConfiguration configuration, Direction direction)
    {
        if (configuration.Center is null)
            return false;

        List<string> connectableTiles = new List<string>(configuration.Center.ConnectableTiles);
        switch (direction)
        {
            case Direction.Left:
                return connectableTiles.Contains(configuration.Left?.Id);
            case Direction.Right:
                return connectableTiles.Contains(configuration.Right?.Id);
            case Direction.Up:
                return connectableTiles.Contains(configuration.Up?.Id);
            case Direction.Down:
                return connectableTiles.Contains(configuration.Down?.Id);
            case Direction.LeftUp:
                return connectableTiles.Contains(configuration.LeftTop?.Id);
            case Direction.RightUp:
                return connectableTiles.Contains(configuration.RightTop?.Id);
            case Direction.LeftDown:
                return connectableTiles.Contains(configuration.LeftBottom?.Id);
            case Direction.RightDown:
                return connectableTiles.Contains(configuration.RightBottom?.Id);
            default:
                return false;
        }
    }
}

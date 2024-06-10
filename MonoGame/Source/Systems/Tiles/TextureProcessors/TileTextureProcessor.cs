using System;
using System.Collections.Generic;
using MonoGame_Common.Util.Enum;
using MonoGame.Source.Systems.Tiles.Interfaces;
using MonoGame.Source.Systems.Tiles.Utils;

namespace MonoGame.Source.Systems.Tiles.TextureProcessors;

public class TileTextureProcessor : ITileTextureProcessor
{
    public virtual (int TextureCoordinateX, int TextureCoordinateY) Process(TileNeighborConfiguration configuration)
    {
        throw new NotImplementedException();
    }

    public bool CanConnect(TileNeighborConfiguration configuration, Direction direction)
    {
        return IsOfSameType(configuration, direction) || IsWhitelisted(configuration, direction);
    }

    public bool IsOfSameType(TileNeighborConfiguration configuration, Direction direction)
    {
        return configuration.Center is not null
               && direction switch
               {
                   Direction.Left => configuration.Left?.GetType() == configuration.Center.GetType(),
                   Direction.Right => configuration.Right?.GetType() == configuration.Center.GetType(),
                   Direction.Up => configuration.Up?.GetType() == configuration.Center.GetType(),
                   Direction.Down => configuration.Down?.GetType() == configuration.Center.GetType(),
                   Direction.LeftUp => configuration.LeftTop?.GetType() == configuration.Center.GetType(),
                   Direction.RightUp => configuration.RightTop?.GetType() == configuration.Center.GetType(),
                   Direction.LeftDown => configuration.LeftBottom?.GetType() == configuration.Center.GetType(),
                   Direction.RightDown => configuration.RightBottom?.GetType() == configuration.Center.GetType(),
                   _ => false
               };
    }

    public bool IsWhitelisted(TileNeighborConfiguration configuration, Direction direction)
    {
        if (configuration.Center is null) return false;

        var connectableTiles = new List<string>(configuration.Center.Tile.ConnectableTiles);
        return direction switch
        {
            Direction.Left => connectableTiles.Contains(configuration.Left?.Tile.Id),
            Direction.Right => connectableTiles.Contains(configuration.Right?.Tile.Id),
            Direction.Up => connectableTiles.Contains(configuration.Up?.Tile.Id),
            Direction.Down => connectableTiles.Contains(configuration.Down?.Tile.Id),
            Direction.LeftUp => connectableTiles.Contains(configuration.LeftTop?.Tile.Id),
            Direction.RightUp => connectableTiles.Contains(configuration.RightTop?.Tile.Id),
            Direction.LeftDown => connectableTiles.Contains(configuration.LeftBottom?.Tile.Id),
            Direction.RightDown => connectableTiles.Contains(configuration.RightBottom?.Tile.Id),
            _ => false
        };
    }
}
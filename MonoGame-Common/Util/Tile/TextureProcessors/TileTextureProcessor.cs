using MonoGame_Common.Util.Enum;

namespace MonoGame_Common.Util.Tile.TextureProcessors;

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
        if (configuration.Center is null)
        {
            return false;
        }

        var connectableTiles = new List<string>(configuration.Center.ConnectableTiles);
        return direction switch
        {
            Direction.Left => connectableTiles.Contains(configuration.Left?.Id ?? ""),
            Direction.Right => connectableTiles.Contains(configuration.Right?.Id ?? ""),
            Direction.Up => connectableTiles.Contains(configuration.Up?.Id ?? ""),
            Direction.Down => connectableTiles.Contains(configuration.Down?.Id ?? ""),
            Direction.LeftUp => connectableTiles.Contains(configuration.LeftTop?.Id ?? ""),
            Direction.RightUp => connectableTiles.Contains(configuration.RightTop?.Id ?? ""),
            Direction.LeftDown => connectableTiles.Contains(configuration.LeftBottom?.Id ?? ""),
            Direction.RightDown => connectableTiles.Contains(configuration.RightBottom?.Id ?? ""),
            _ => false
        };
    }
}
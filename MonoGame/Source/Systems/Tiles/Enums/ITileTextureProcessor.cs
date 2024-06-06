using MonoGame.Source.Systems.Tiles.Utils;

namespace MonoGame;

public interface ITileTextureProcessor
{

    public abstract (int, int) Process(TileNeighborConfiguration configuration);

    public abstract bool CanConnect(TileNeighborConfiguration configuration, Direction direction);

    public abstract bool IsOfSameType(TileNeighborConfiguration configuration, Direction direction);

    public abstract bool IsWhitelisted(TileNeighborConfiguration configuration, Direction direction);
}

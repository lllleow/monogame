using MonoGame_Common.Util.Enum;
using MonoGame.Source.Systems.Tiles.Utils;

namespace MonoGame.Source.Systems.Tiles.Interfaces;

public interface ITileTextureProcessor
{
    public abstract (int TextureCoordinateX, int TextureCoordinateY) Process(TileNeighborConfiguration configuration);

    public abstract bool CanConnect(TileNeighborConfiguration configuration, Direction direction);

    public abstract bool IsOfSameType(TileNeighborConfiguration configuration, Direction direction);

    public abstract bool IsWhitelisted(TileNeighborConfiguration configuration, Direction direction);
}

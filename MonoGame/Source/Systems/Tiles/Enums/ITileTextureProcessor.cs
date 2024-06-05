using System;
using MonoGame.Source.Systems.Tiles.Utils;

namespace MonoGame;

public interface ITileTextureProcessor
{
    /// <summary>
    /// Processes the given <paramref name="configuration"/> and returns a tuple of two integers.
    /// </summary>
    /// <param name="configuration">The tile neighbor configuration to process.</param>
    /// <returns>A tuple containing two integers.</returns>
    public abstract (int, int) Process(TileNeighborConfiguration configuration);

    /// <summary>
    /// Checks if the given tile neighbor configuration can connect in the specified direction.
    /// </summary>
    /// <param name="configuration">The tile neighbor configuration to check.</param>
    /// <param name="direction">The direction to check.</param>
    /// <returns>True if the configuration can connect in the specified direction, otherwise false.</returns>
    public abstract bool CanConnect(TileNeighborConfiguration configuration, Direction direction);

    /// <summary>
    /// Checks if the tile in the given direction is of the same type as the center tile.
    /// </summary>
    /// <param name="configuration">The tile neighbor configuration to check.</param>
    /// <param name="direction">The direction to check.</param>
    /// <returns>True if the tile in the given direction is of the same type as the center tile, otherwise false.</returns>
    public abstract bool IsOfSameType(TileNeighborConfiguration configuration, Direction direction);

    /// <summary>
    /// Checks if the tile in the given direction is whitelisted in the center tile's connectable tiles list.
    /// </summary>
    /// <param name="configuration">The tile neighbor configuration to check.</param>
    /// <param name="direction">The direction to check.</param>
    /// <returns>True if the tile in the given direction is whitelisted, otherwise false.</returns>
    public abstract bool IsWhitelisted(TileNeighborConfiguration configuration, Direction direction);
}

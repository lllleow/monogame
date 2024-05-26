using System;

namespace MonoGame
{

    /// <summary>
    /// Represents the different layers for drawing tiles.
    /// </summary>
    public enum TileDrawLayer
    {
        /// <summary>
        /// The background layer.
        /// </summary>
        Background,

        /// <summary>
        /// The terrain layer.
        /// </summary>
        Terrain,

        /// <summary>
        /// The tiles layer.
        /// </summary>
        Tiles
    }

    /// <summary>
    /// Provides a set of methods for managing the priority of tile draw layers.
    /// </summary>
    public static class TileDrawLayerPriority
    {
        /// <summary>
        /// Gets the priority of the tile draw layers.
        /// </summary>
        /// <returns>An array of <see cref="TileDrawLayer"/> representing the priority of the tile draw layers.</returns>
        public static TileDrawLayer[] GetPriority()
        {
            return new TileDrawLayer[]
            {
                TileDrawLayer.Background,
                TileDrawLayer.Terrain,
                TileDrawLayer.Tiles
            };
        }
    }
}

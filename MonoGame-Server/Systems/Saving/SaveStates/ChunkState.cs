using System;
using System.Collections.Generic;
using MonoGame.Source.Systems.Chunks.Interfaces;

namespace MonoGame;

public class ChunkState
{
    public List<TileState> Tiles { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public ChunkState()
    {

    }

    public ChunkState(IChunk chunk)
    {
        Tiles = new List<TileState>();
        X = chunk.X;
        Y = chunk.Y;

        foreach (KeyValuePair<TileDrawLayer, ITile[,]> layer in chunk.Tiles)
        {
            if (layer.Key != TileDrawLayer.Background)
            {
                for (int x = 0; x < layer.Value.GetLength(0); x++)
                {
                    for (int y = 0; y < layer.Value.GetLength(1); y++)
                    {
                        ITile tile = layer.Value[x, y];
                        if (tile != null)
                        {
                            Tiles.Add(new TileState(layer.Key, tile));
                        }
                    }
                }
            }
        }
    }
}

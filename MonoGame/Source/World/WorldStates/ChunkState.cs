using System;
using System.Collections.Generic;
using LiteNetLib.Utils;
using MonoGame.Source.Systems.Chunks.Interfaces;

namespace MonoGame;

public class ChunkState : INetSerializable
{
    public List<TileState> Tiles { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public ChunkState()
    {
        Tiles = new List<TileState>();
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

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(X);
        writer.Put(Y);
        writer.Put(Tiles.Count);
        foreach (TileState tile in Tiles)
        {
            tile.Serialize(writer);
        }
    }

    public void Deserialize(NetDataReader reader)
    {
        X = reader.GetInt();
        Y = reader.GetInt();
        int tileCount = reader.GetInt();
        Tiles = new List<TileState>();
        for (int i = 0; i < tileCount; i++)
        {
            TileState tile = new TileState();
            tile.Deserialize(reader);
            Tiles.Add(tile);
        }
    }
}

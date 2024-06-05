using System;
using System.Collections.Generic;
using System.Linq;
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

    public ChunkState(int x, int y)
    {
        Tiles = new List<TileState>();
        X = x;
        Y = y;
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

    public bool SetTile(string tileId, TileDrawLayer layer, int posX, int posY)
    {
        TileState tile = Tiles.FirstOrDefault(x => x.LocalX == posX && x.LocalY == posY && x.Layer == layer);

        if (tile != null)
        {
            return false;
        }
        else
        {
            Tiles.Add(new TileState(tileId, layer, posX, posY));
            return true;
        }
    }

    public bool DestroyTile(TileDrawLayer layer, int posX, int posY)
    {
        TileState tile = Tiles.FirstOrDefault(x => x.LocalX == posX && x.LocalY == posY && x.Layer == layer);
        if (tile != null)
        {
            Tiles.Remove(tile);
            return true;
        }

        return false;
    }

    public TileState GetTile(TileDrawLayer layer, int posX, int posY)
    {
        return Tiles.FirstOrDefault(x => x.LocalX == posX && x.LocalY == posY && x.Layer == layer);
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

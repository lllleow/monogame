using System.Collections.Concurrent;
using System.Numerics;
using LiteNetLib.Utils;
using MonoGame_Common.Enums;
using MonoGame_Common.Systems.Scripts;
using MonoGame_Common.Util.Tile;
using MonoGame_Common.Util.Tile.TileComponents;

namespace MonoGame_Common.States;

public class ChunkState : INetSerializable
{
    public ChunkState()
    {
        Tiles = new ConcurrentDictionary<TileDrawLayer, TileState?[,]>();
        foreach (TileDrawLayer layer in TileDrawLayerPriority.GetPriority())
        {
            Tiles[layer] = new TileState[SizeX, SizeY];
        }
    }

    public ChunkState(int x, int y)
    {
        Tiles = new ConcurrentDictionary<TileDrawLayer, TileState?[,]>();
        foreach (TileDrawLayer layer in TileDrawLayerPriority.GetPriority())
        {
            Tiles[layer] = new TileState[SizeX, SizeY];
        }

        X = x;
        Y = y;
    }

    public static int SizeX { get; set; } = 16;
    public static int SizeY { get; set; } = 16;
    public ConcurrentDictionary<TileDrawLayer, TileState?[,]> Tiles { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(X);
        writer.Put(Y);
        writer.Put(Tiles.Count);

        foreach (var layer in Tiles.Keys)
        {
            writer.Put((byte)layer);

            List<PositionedTileHelper> positionedTileStates = new List<PositionedTileHelper>();
            for (var i = 0; i < Tiles[layer].GetLength(0); i++)
            {
                for (var j = 0; j < Tiles[layer].GetLength(1); j++)
                {
                    TileState? state = Tiles[layer][i, j];
                    if (state != null)
                    {
                        PositionedTileHelper positionedTile = new PositionedTileHelper(state, this, i, j);
                        positionedTileStates.Add(positionedTile);
                    }
                }
            }

            writer.Put(positionedTileStates.Count);
            foreach (var tileState in positionedTileStates)
            {
                writer.Put(tileState.PosX);
                writer.Put(tileState.PosY);
                tileState.Tile.Serialize(writer);
            }
        }
    }

    public void Deserialize(NetDataReader reader)
    {
        X = reader.GetInt();
        Y = reader.GetInt();
        int layerCount = reader.GetInt();

        for (int i = 0; i < layerCount; i++)
        {
            TileDrawLayer layer = (TileDrawLayer)reader.GetByte();
            int tileCount = reader.GetInt();

            for (int j = 0; j < tileCount; j++)
            {
                int posX = reader.GetInt();
                int posY = reader.GetInt();
                TileState tile = new TileState();
                tile.Deserialize(reader);

                Tiles[layer][posX, posY] = tile;
            }
        }
    }

    public bool SetTile(string tileId, TileDrawLayer layer, int posX, int posY)
    {
        var tile = Tiles[layer][posX, posY];

        if (tile != null)
        {
            return false;
        }

        Tiles[layer][posX, posY] = new TileState(tileId);
        return true;
    }

    public bool DestroyTile(TileDrawLayer layer, int posX, int posY)
    {
        var tile = Tiles[layer][posX, posY];
        if (tile != null)
        {
            Tiles[layer][posX, posY] = null;
            return true;
        }

        return false;
    }

    public TileState? GetTile(TileDrawLayer layer, int posX, int posY)
    {
        return Tiles[layer][posX, posY];
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        var worldX = (X * SizeX) + x;
        var worldY = (Y * SizeY) + y;
        return new Vector2(worldX, worldY);
    }

    public Vector2 GetWorldPosition(Vector2 localPosition)
    {
        return GetWorldPosition((int)localPosition.X, (int)localPosition.Y);
    }

    public Vector2 GetTilePosition(TileState tile)
    {
        return GetTilePositionAndLayer(tile).Position ?? Vector2.Zero;
    }

    public (TileDrawLayer Layer, Vector2? Position) GetTilePositionAndLayer(TileState tile)
    {
        foreach (var layer in Tiles.Keys)
        {
            var tilePosition = GetTilePosition(layer, tile);
            if (tilePosition != null)
            {
                return (layer, tilePosition);
            }
        }

        return (TileDrawLayer.Tiles, Vector2.Zero);
    }

    public Vector2? GetTilePosition(TileDrawLayer layer, TileState tile)
    {
        for (var x = 0; x < SizeX; x++)
        {
            for (var y = 0; y < SizeY; y++)
            {
                var tileState = Tiles[layer][x, y];
                if (tileState != null && tileState == tile)
                {
                    return new Vector2(x, y);
                }
            }
        }

        return null;
    }
}
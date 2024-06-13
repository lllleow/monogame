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
        Tiles = [];
    }

    public ChunkState(int x, int y)
    {
        Tiles = [];
        X = x;
        Y = y;
    }

    public static int SizeX { get; set; } = 16;
    public static int SizeY { get; set; } = 16;
    public List<TileState> Tiles { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(X);
        writer.Put(Y);
        writer.Put(Tiles.Count);
        foreach (var tile in Tiles)
        {
            tile.Serialize(writer);
        }
    }

    public void Deserialize(NetDataReader reader)
    {
        X = reader.GetInt();
        Y = reader.GetInt();
        var tileCount = reader.GetInt();
        Tiles = [];
        for (var i = 0; i < tileCount; i++)
        {
            var tile = new TileState();
            tile.Deserialize(reader);
            Tiles.Add(tile);
        }
    }

    public bool SetTile(string tileId, TileDrawLayer layer, int posX, int posY)
    {
        var tile = Tiles.FirstOrDefault(x => x.LocalX == posX && x.LocalY == posY && x.Layer == layer);

        if (tile != null)
        {
            return false;
        }

        Tiles.Add(new TileState(tileId, layer, X, Y, posX, posY));
        return true;
    }

    public bool DestroyTile(TileDrawLayer layer, int posX, int posY)
    {
        var tile = Tiles.FirstOrDefault(x => x.LocalX == posX && x.LocalY == posY && x.Layer == layer);
        if (tile != null)
        {
            _ = Tiles.Remove(tile);
            return true;
        }

        return false;
    }

    public TileState GetTile(TileDrawLayer layer, int posX, int posY)
    {
        return Tiles.FirstOrDefault(x => x.LocalX == posX && x.LocalY == posY && x.Layer == layer);
    }

    public Vector2 GetWorldPosition(int x, int y)
    {
        var worldX = (X * SizeX) + x;
        var worldY = (Y * SizeY) + y;
        return new Vector2(worldX, worldY);
    }
}
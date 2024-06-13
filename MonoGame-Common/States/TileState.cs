using LiteNetLib.Utils;
using MonoGame_Common.Enums;
using MonoGame_Common.States.TileComponents;
using MonoGame_Common.Systems.Scripts;
using MonoGame_Common.Systems.Tiles.Interfaces;
using MonoGame_Common.Util.Tile.TileComponents;

namespace MonoGame_Common.States;

public class TileState : INetSerializable
{
    public TileState()
    {
    }

    public TileState(string id, TileDrawLayer layer, int chunkX, int chunkY, int x, int y)
    {
        Id = id;
        LocalX = x;
        LocalY = y;
        Layer = layer;
        ChunkX = chunkX;
        ChunkY = chunkY;
    }

    public static int PixelSizeX { get; set; } = 16;
    public static int PixelSizeY { get; set; } = 16;
    public string Id { get; set; }
    public int? ChunkX { get; set; }
    public int? ChunkY { get; set; }
    public int? LocalX { get; set; }
    public int? LocalY { get; set; }
    public TileDrawLayer Layer { get; set; }
    public List<TileComponentState> Components { get; set; } = [];

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(Id);
        writer.Put(LocalX ?? 0);
        writer.Put(LocalY ?? 0);
        writer.Put(ChunkX ?? 0);
        writer.Put(ChunkY ?? 0);
        writer.Put((byte)Layer);
    }

    public void Deserialize(NetDataReader reader)
    {
        Id = reader.GetString();
        LocalX = reader.GetInt();
        LocalY = reader.GetInt();
        ChunkX = reader.GetInt();
        ChunkY = reader.GetInt();
        Layer = (TileDrawLayer)reader.GetByte();
    }

    public CommonTile? GetCommonTile()
    {
        return TileRegistry.GetTile(Id);
    }

    public void AddComponent(TileComponentState component)
    {
        Components.Add(component);
    }

    public void RemoveComponent(TileComponentState component)
    {
        _ = Components.Remove(component);
    }

    public T GetComponent<T>()
    where T : TileComponentState
    {
        return (T)Components.FirstOrDefault(x => x.GetType() == typeof(T));
    }

    public bool HasComponent<T>()
    where T : TileComponentState
    {
        return Components.Any(x => x.GetType() == typeof(T));
    }
}
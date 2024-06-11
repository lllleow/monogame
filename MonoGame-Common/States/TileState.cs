using LiteNetLib.Utils;
using MonoGame_Common.Enums;

namespace MonoGame_Common.States;

public class TileState : INetSerializable
{
    public TileState()
    {
    }

    public TileState(string id, TileDrawLayer layer, int x, int y)
    {
        Id = id;
        LocalX = x;
        LocalY = y;
        Layer = layer;
    }

    public static int PixelSizeX { get; set; } = 16;
    public static int PixelSizeY { get; set; } = 16;
    public string Id { get; set; }
    public int? LocalX { get; set; }
    public int? LocalY { get; set; }
    public TileDrawLayer Layer { get; set; }
    public List<TileComponentState> Components { get; set; } = new();

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(Id);
        writer.Put(LocalX ?? 0);
        writer.Put(LocalY ?? 0);
        writer.Put((byte)Layer);
        writer.Put(Components.Count);
        foreach (var component in Components)
        {
            component.Serialize(writer);
        }
    }

    public void Deserialize(NetDataReader reader)
    {
        Id = reader.GetString();
        LocalX = reader.GetInt();
        LocalY = reader.GetInt();
        Layer = (TileDrawLayer)reader.GetByte();
        var componentCount = reader.GetInt();
        for (var i = 0; i < componentCount; i++)
        {
            var component = new TileComponentState();
            component.Deserialize(reader);
            Components.Add(component);
        }
    }

    public void AddComponent(TileComponentState component)
    {
        Components.Add(component);
    }

    public void RemoveComponent<T>()
    where T : TileComponentState
    {
        Components.RemoveAll(component => component is T);
    }

    public T GetComponent<T>()
    where T : TileComponentState
    {
        return (T)Components.FirstOrDefault(component => component is T) ?? default!;
    }

    public bool HasComponent<T>()
    where T : TileComponentState
    {
        return Components.Any(component => component is T);
    }
}
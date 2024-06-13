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
        InitializeComponents();
    }

    public TileState(string id)
    {
        Id = id;
        InitializeComponents();
    }

    public string Id { get; set; }
    public List<TileComponentState> Components { get; set; } = [];

    public void InitializeComponents()
    {
        CommonTile? commonTile = GetCommonTile();
        if (commonTile != null)
        {
            foreach (var component in commonTile.Components)
            {
                TileComponentState tileComponentState = component.GetTileComponentState();
                tileComponentState.TileState = this;

                AddComponent(tileComponentState);
            }
        }
    }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(Id);
    }

    public void Deserialize(NetDataReader reader)
    {
        Id = reader.GetString();
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
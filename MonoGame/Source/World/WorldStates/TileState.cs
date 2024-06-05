using System;
using LiteNetLib.Utils;

namespace MonoGame;

public class TileState : INetSerializable
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int? LocalX { get; set; }
    public int? LocalY { get; set; }
    public TileDrawLayer? Layer { get; set; }

    public TileState()
    {

    }

    public TileState(TileDrawLayer layer, ITile tile)
    {
        Id = tile.Id;
        Name = tile.Name;
        LocalX = tile.LocalX;
        LocalY = tile.LocalY;
        Layer = layer;
    }

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(Id);
        writer.Put(Name);
        writer.Put(LocalX ?? 0);
        writer.Put(LocalY ?? 0);
        writer.Put((byte)Layer);
    }

    public void Deserialize(NetDataReader reader)
    {
        Id = reader.GetString();
        Name = reader.GetString();
        LocalX = reader.GetInt();
        LocalY = reader.GetInt();
        Layer = (TileDrawLayer)reader.GetByte();
    }
}

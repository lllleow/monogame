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

    public void Serialize(NetDataWriter writer)
    {
        writer.Put(Id);
        writer.Put(LocalX ?? 0);
        writer.Put(LocalY ?? 0);
        writer.Put((byte)Layer);
    }

    public void Deserialize(NetDataReader reader)
    {
        Id = reader.GetString();
        LocalX = reader.GetInt();
        LocalY = reader.GetInt();
        Layer = (TileDrawLayer)reader.GetByte();
    }
}
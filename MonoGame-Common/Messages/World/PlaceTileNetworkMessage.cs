using LiteNetLib.Utils;
using MonoGame_Common.Attributes;
using MonoGame_Common.Enums;

namespace MonoGame_Common.Messages.World;

[NetworkMessage(10)]
public class PlaceTileNetworkMessage : NetworkMessage
{
    public PlaceTileNetworkMessage()
    {
    }

    required public string TileId { get; set; }
    public TileDrawLayer Layer { get; set; }
    public int PosX { get; set; }
    public int PosY { get; set; }

    public override void Deserialize(NetDataReader reader)
    {
        TileId = reader.GetString();
        Layer = (TileDrawLayer)reader.GetByte();
        PosX = reader.GetInt();
        PosY = reader.GetInt();
    }

    public override NetDataWriter Serialize()
    {
        var data = new NetDataWriter();
        data.Put(GetNetworkTypeId());
        data.Put(TileId);
        data.Put((byte)Layer);
        data.Put(PosX);
        data.Put(PosY);
        return data;
    }
}
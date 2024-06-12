using LiteNetLib.Utils;
using MonoGame_Common.Attributes;
using MonoGame_Common.Enums;

namespace MonoGame_Common.Messages.World;

[NetworkMessage(11)]
public class DeleteTileNetworkMessage : NetworkMessage
{
    public DeleteTileNetworkMessage()
    {
    }

    public DeleteTileNetworkMessage(TileDrawLayer layer, int posX, int posY)
    {
        Layer = layer;
        PosX = posX;
        PosY = posY;
    }

    public TileDrawLayer Layer { get; set; }
    public int PosX { get; set; }
    public int PosY { get; set; }

    public override void Deserialize(NetDataReader reader)
    {
        Layer = (TileDrawLayer)reader.GetByte();
        PosX = reader.GetInt();
        PosY = reader.GetInt();
    }

    public override NetDataWriter Serialize()
    {
        var data = new NetDataWriter();
        data.Put(GetNetworkTypeId());
        data.Put((byte)Layer);
        data.Put(PosX);
        data.Put(PosY);
        return data;
    }
}
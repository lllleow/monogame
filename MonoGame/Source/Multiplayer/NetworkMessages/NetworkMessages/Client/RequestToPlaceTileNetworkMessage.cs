using LiteNetLib.Utils;
using MonoGame.Source.Rendering.Enum;

namespace MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Client
{
    [NetworkMessage(9)]
    public class RequestToPlaceTileNetworkMessage : NetworkMessage
    {
        public string TileId { get; set; }
        public TileDrawLayer Layer { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }

        public RequestToPlaceTileNetworkMessage() : base()
        {
        }

        public RequestToPlaceTileNetworkMessage(string tileId, TileDrawLayer layer, int posX, int posY) : base()
        {
            TileId = tileId;
            Layer = layer;
            PosX = posX;
            PosY = posY;
        }

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
}
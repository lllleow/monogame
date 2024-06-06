using LiteNetLib.Utils;

namespace MonoGame
{
    public class RequestToPlaceTileNetworkMessage : NetworkMessage
    {
        public string TileId { get; set; }
        public TileDrawLayer Layer { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }

        public RequestToPlaceTileNetworkMessage()
        {
        }

        public RequestToPlaceTileNetworkMessage(string tileId, TileDrawLayer layer, int posX, int posY)
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
            NetDataWriter data = new NetDataWriter();
            data.Put((byte)NetworkMessageTypes.RequestToPlaceTileNetworkMessage);
            data.Put(TileId);
            data.Put((byte)Layer);
            data.Put(PosX);
            data.Put(PosY);
            return data;
        }
    }
}
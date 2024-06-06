using LiteNetLib.Utils;

namespace MonoGame
{
    public class PlaceTileNetworkMessage : NetworkMessage
    {
        public string TileId;
        public TileDrawLayer Layer;
        public int PosX;
        public int PosY;

        public PlaceTileNetworkMessage()
        {
        }

        public PlaceTileNetworkMessage(string tileId, TileDrawLayer layer, int posX, int posY)
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
            data.Put((byte)NetworkMessageTypes.PlaceTileNetworkMessage);
            data.Put(TileId);
            data.Put((byte)Layer);
            data.Put(PosX);
            data.Put(PosY);
            return data;
        }
    }
}
using LiteNetLib.Utils;
using MonoGame.Source.Rendering.Enum;

namespace MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Server
{
    public class PlaceTileNetworkMessage : NetworkMessage
    {
        public string TileId { get; set; }
        public TileDrawLayer Layer { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }

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
            var data = new NetDataWriter();
            data.Put((byte)NetworkMessageTypes.PlaceTileNetworkMessage);
            data.Put(TileId);
            data.Put((byte)Layer);
            data.Put(PosX);
            data.Put(PosY);
            return data;
        }
    }
}
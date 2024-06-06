using LiteNetLib.Utils;
using MonoGame.Source.Rendering.Enum;

namespace MonoGame.Source.Multiplayer.NetworkMessages.NetworkMessages.Server
{
    public class DeleteTileNetworkMessage : NetworkMessage
    {
        public TileDrawLayer Layer { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }

        public DeleteTileNetworkMessage()
        {
        }

        public DeleteTileNetworkMessage(TileDrawLayer layer, int posX, int posY)
        {
            Layer = layer;
            PosX = posX;
            PosY = posY;
        }

        public override void Deserialize(NetDataReader reader)
        {
            Layer = (TileDrawLayer)reader.GetByte();
            PosX = reader.GetInt();
            PosY = reader.GetInt();
        }

        public override NetDataWriter Serialize()
        {
            var data = new NetDataWriter();
            data.Put((byte)NetworkMessageTypes.DeleteTileNetworkMessage);
            data.Put((byte)Layer);
            data.Put(PosX);
            data.Put(PosY);
            return data;
        }
    }
}

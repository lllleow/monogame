using System;
using LiteNetLib.Utils;

namespace MonoGame
{
    public class DeleteTileNetworkMessage : NetworkMessage
    {
        public TileDrawLayer Layer;
        public int PosX;
        public int PosY;

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
            NetDataWriter data = new NetDataWriter();
            data.Put((byte)NetworkMessageTypes.DeleteTileNetworkMessage);
            data.Put((byte)Layer);
            data.Put(PosX);
            data.Put(PosY);
            return data;
        }
    }
}

using LiteNetLib.Utils;
using MonoGame.Source.Rendering.Enum;
using MonoGame_Common.Messages;

namespace MonoGame.Source.Multiplayer.Messages.World
{
    [NetworkMessage(11)]
    public class DeleteTileNetworkMessage : NetworkMessage
    {
        public TileDrawLayer Layer { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }

        public DeleteTileNetworkMessage() : base()
        {
        }

        public DeleteTileNetworkMessage(TileDrawLayer layer, int posX, int posY) : base()
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
            data.Put(GetNetworkTypeId());
            data.Put((byte)Layer);
            data.Put(PosX);
            data.Put(PosY);
            return data;
        }
    }
}

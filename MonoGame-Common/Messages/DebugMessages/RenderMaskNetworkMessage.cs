using System;
using System.Drawing;
using LiteNetLib.Utils;
using MonoGame_Common.Attributes;
using MonoGame_Common.Messages;

namespace MonoGame
{
    [NetworkMessage(18)]
    public class RenderMaskNetworkMessage : NetworkMessage
    {
        public Rectangle Rectangle { get; set; }
        public bool[,] Mask { get; set; }

        public RenderMaskNetworkMessage()
        {
        }

        public RenderMaskNetworkMessage(Rectangle rectangle, bool[,] mask)
        {
            Rectangle = rectangle;
            Mask = mask;
        }

        public override void Deserialize(NetDataReader reader)
        {
            Rectangle = new Rectangle(reader.GetInt(), reader.GetInt(), reader.GetInt(), reader.GetInt());
            Mask = new bool[reader.GetInt(), reader.GetInt()];
            for (int i = 0; i < Mask.GetLength(0); i++)
            {
                for (int j = 0; j < Mask.GetLength(1); j++)
                {
                    Mask[i, j] = reader.GetBool();
                }
            }
        }

        public override NetDataWriter Serialize()
        {
            NetDataWriter data = new NetDataWriter();
            data.Put(GetNetworkTypeId());
            data.Put(Rectangle.X);
            data.Put(Rectangle.Y);
            data.Put(Rectangle.Width);
            data.Put(Rectangle.Height);
            data.Put(Mask.GetLength(0));
            data.Put(Mask.GetLength(1));
            for (int i = 0; i < Mask.GetLength(0); i++)
            {
                for (int j = 0; j < Mask.GetLength(1); j++)
                {
                    data.Put(Mask[i, j]);
                }
            }
            return data;
        }
    }
}
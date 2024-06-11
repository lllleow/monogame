using LiteNetLib.Utils;

namespace MonoGame_Common;

public class TextureRendererTileComponentState : TileComponentState
{
    public int TextureX { get; set; }

    public int TextureY { get; set; }

    public override void Deserialize(NetDataReader reader)
    {
        TextureX = reader.GetInt();
        TextureY = reader.GetInt();
    }

    public override void Serialize(NetDataWriter writer)
    {
        writer.Put(TextureX);
        writer.Put(TextureY);
    }
}
